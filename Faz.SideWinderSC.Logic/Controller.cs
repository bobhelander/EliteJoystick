using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Base class for a controller that uses HID for its communication.
    /// </summary>
    public class Controller : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The encapsulated stream.
        /// </summary>
        private readonly HidStream stream;

        /// <summary>
        /// The number of buffers available.
        /// </summary>
        private ulong readBufferCount = 3;

        /// <summary>
        /// The buffers used when reading the stream.
        /// </summary>
        private List<byte[]> readBuffers = new List<byte[]>();

        /// <summary>
        /// Process every report or check against previous
        /// </summary>
        public bool ProcessAllReports { get; set; }

        /// <summary>
        /// Flag to keep reading the serial stream.
        /// </summary>
        public bool ContinueProcessing { get; set; }

        /// <summary>
        /// Remove jitter from continuous button events
        /// </summary>
        public Stopwatch Stopwatch { get; set; }

        /// <summary>
        /// Serial reading task
        /// </summary>
        private Task SerialProcessingTask { get; set; }

        /// <summary>
        /// The buffer used when getting the feature report.
        /// </summary>
        private readonly byte[] featureBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        /// <param name="devicePath">The path of the device.</param>
        protected Controller(string devicePath)
        {
            this.stream = new HidStream(devicePath);
            this.featureBuffer = new byte[this.FeatureLength];
            ProcessAllReports = true;
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        /// <summary>
        /// Occurs when a new entry is read.
        /// </summary>
        public event EventHandler<ReadEventArgs> Read;

        /// <summary>
        /// Gets the prefered length for the read buffer.
        /// </summary>
        protected int ReadLength
        {
            get { return this.stream.Capabilities.InputReportByteLength; }
        }

        /// <summary>
        /// Gets the prefered length for the write buffer.
        /// </summary>
        protected int WriteLength
        {
            get { return this.stream.Capabilities.OutputReportByteLength; }
        }

        /// <summary>
        /// Write a buffer to the USB device
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        public void Write(byte[] buffer, int length)
        {
            if (this.stream.CanWrite)
            {
                Task.Factory.StartNew(() => this.WriteToDevice(buffer, length),
                    CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default)
                    .ContinueWith(t => { log.Error($"Controller Write  Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        /// <summary>
        /// Gets the prefered length for the feature buffer.
        /// </summary>
        public int FeatureLength
        {
            get { return this.stream.Capabilities.FeatureReportByteLength; }
        }

        /// <summary>
        /// Gets or sets the feature report of the controller.
        /// </summary>
        public byte[] Feature
        {
            get
            {
                this.stream.ReadFeature(this.featureBuffer, 0, this.FeatureLength);
                return this.featureBuffer;
            }

            set
            {
                this.stream.WriteFeature(value, 0, this.FeatureLength);
            }
        }

        /// <summary>
        /// Gets a feature value
        /// </summary>
        public byte[] FeatureValue
        {
            get
            {
                this.stream.ReadFeature(this.featureBuffer, 0, this.FeatureLength, 0);
                return this.featureBuffer;
            }

            set
            {
                this.stream.WriteFeature(value, 0, this.FeatureLength, 0);
            }
        }

        /// <summary>
        /// Initialises the device for asynchronous reads.
        /// </summary>
        public virtual void Initialize()
        {
            for (ulong count = 0; count < readBufferCount; count++)
                this.readBuffers.Add(new byte[this.ReadLength]);

            ContinueProcessing = true;
            SerialProcessingTask = Task.Factory.StartNew(() => ReadSerial(),
                CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(t => { log.Error($"Controller Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// Retrieves all the device paths for a specified vendor and product ids.
        /// </summary>
        /// <param name="vendorId">The vendor id.</param>
        /// <param name="productId">The product id.</param>
        /// <returns>
        /// The device paths associated to <paramref name="vendorId"/> and
        /// <paramref name="productId"/>.
        /// </returns>
        protected static ICollection<string> RetrieveAllDevicePath(int vendorId, int productId)
        {
            // build the path search string
            string search = string.Format("vid_{0:x4}&pid_{1:x4}", vendorId, productId);
            return Device.GetInterfaceDevices(HidStream.HidGuid)
                .Where(d => d.DevicePath != null && d.DevicePath.Contains(search))
                .Select(d => d.DevicePath)
                .ToList();
        }

        /// <summary>
        /// Reads a message from the device
        /// </summary>
        /// <returns></returns>
        private async Task ReadSerial()
        {
            ulong counter = 0;
            while (ContinueProcessing)
            {
                // Round robin the available buffers
                var bufferIndex = (int)(counter % readBufferCount);

                var size = await this.stream.ReadAsync(this.readBuffers[bufferIndex], 0, this.ReadLength);
                await ProcessSerialMessage(size, this.readBuffers[bufferIndex], this.ReadLength, counter);

                counter++;
            }
        }

        /// <summary>
        /// Processes the buffer received from the device
        /// </summary>
        /// <param name="size"></param>
        /// <param name="buffer"></param>
        /// <param name="requestedLength"></param>
        /// <param name="stateCounter"></param>
        /// <returns></returns>
        private async Task<bool> ProcessSerialMessage(int size, byte[] buffer, int requestedLength, ulong stateCounter)
        {
            if (size != requestedLength)
            {
                // Throw this read out.  
                log.Error($"Read Invalid Size {requestedLength}, Actual size {size}");
                return false;
            }
            try
            {
                if (this.Read != null)
                {
                    // Copy the buffer first
                    //var buffer = await CopyBuffer(this.readBuffer);

                    if (ProcessAllReports)
                    {
                        // Process the change
                        CallReadEventAsync(buffer, size, stateCounter);
                    }
                    else if (stateCounter > 0)
                    {
                        var lastBufferIndex = (int)((stateCounter - 1) % readBufferCount);
                        var lastReadBuffer = this.readBuffers[lastBufferIndex];

                        // Remove jitter around the buttons
                        if (false == buffer.SequenceEqual(lastReadBuffer))
                        {
                            // The read buffer changed since the last read
                            // Restart the stopwatch
                            Stopwatch.Restart();
                        }

                        // After the state remains stable
                        if (Stopwatch.ElapsedMilliseconds > 100)
                        {
                            if (buffer.SequenceEqual(lastReadBuffer))
                            {
                                // Process the change
                                CallReadEventAsync(buffer, size, stateCounter);
                            }
                        }
                    }
                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return await Task.FromResult(false);
        }

        /// <summary>
        /// Calls the event handler
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="stateCounter"></param>
        private void CallReadEventAsync(byte[] buffer, int size, ulong stateCounter)
        {
            // Allow this to process in the thread pool
            Task.Run(() => Read(this, new ReadEventArgs(buffer, size, stateCounter)))
                .ContinueWith(t => { log.Error($"Read EventHandler Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// Write buffer to the stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private async Task WriteToDevice(byte[] buffer, int length)
        {
            await this.stream?.WriteAsync(buffer, 0, length);
        }

        /// <summary>
        /// Releases the associated ressources.
        /// </summary>
        public void Dispose()
        {
            this.stream?.Dispose();
        }
    }
}
