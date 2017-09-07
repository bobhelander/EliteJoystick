using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Base class for a controller that uses HID for its communication.
    /// </summary>
    public class Controller : IDisposable
    {
        /// <summary>
        /// The encapsulated stream.
        /// </summary>
        private readonly HidStream stream;

        /// <summary>
        /// The buffer used when reading the stream.
        /// </summary>
        private byte[] readBuffer;

        /// <summary>
        /// Last buffer read
        /// </summary>
        private byte[] lastReadBuffer;

        /// <summary>
        /// Last processed state
        /// </summary>
        private byte[] currentBuffer;

        /// <summary>
        /// Process every report or check against previous
        /// </summary>
        public bool ProcessAllReports { get; set; }

        /// <summary>
        /// Remove jitter from continuous button events
        /// </summary>
        public Stopwatch Stopwatch { get; set; }

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
                BeginAsyncWrite(buffer, length);
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
            this.readBuffer = new byte[this.ReadLength];
            this.lastReadBuffer = new byte[this.ReadLength];
            this.currentBuffer = new byte[this.ReadLength];
            this.BeginAsyncRead((ulong) 0);
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
            return Device.GetInterfaceDevices(HidStream.HidGuid).Where(d => d.DevicePath != null && d.DevicePath.Contains(search)).Select(d => d.DevicePath).ToList();
        }

        /// <summary>
        /// Starts an asynchronous read.
        /// </summary>
        private void BeginAsyncRead(ulong counter)
        {
            this.stream?.BeginRead(this.readBuffer, 0, this.ReadLength, this.OnReadCompleted, counter);
        }

        /// <summary>
        /// Called when a read in completed.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void OnReadCompleted(IAsyncResult asyncResult)
        {
            try
            {
                // call EndRead:
                // - throws any exceptions that happened during the read
                // - returns the number of valid byte in the buffer
                int size = this.stream.EndRead(asyncResult);

                try
                {
                    if (this.Read != null)
                    {
                        if (ProcessAllReports)
                        {
                            // Process the change
                            this.Read(this, new ReadEventArgs(this.readBuffer, size, (ulong)asyncResult.AsyncState));
                        }
                        else
                        {
                            // Remove jitter around the buttons

                            if (false == this.readBuffer.SequenceEqual(this.lastReadBuffer))
                            {
                                // The read buffer changed since the last read
                                // Restart the stopwatch
                                Stopwatch.Restart();
                            }

                            // After the state remains stable
                            if (Stopwatch.ElapsedMilliseconds > 100)
                            {
                                if (false == this.readBuffer.SequenceEqual(this.currentBuffer))
                                {
                                    // Process the change
                                    this.Read(this, new ReadEventArgs(this.readBuffer, size, (ulong)asyncResult.AsyncState));
                                    this.currentBuffer = this.readBuffer.ToArray();
                                }
                            }

                            // Update the last buffer
                            this.lastReadBuffer = this.readBuffer.ToArray();
                        }
                    }
                }
                finally
                {
                    // Start a new read
                    this.BeginAsyncRead((ulong)asyncResult.AsyncState + 1);
                }
            }
            catch (IOException)
            {
                // if we got an IO exception, the device was removed
            }
        }

        /// <summary>
        /// Starts an asynchronous read.
        /// </summary>
        private void BeginAsyncWrite(byte[] buffer, int length)
        {
            this.stream?.BeginWrite(buffer, 0, length, this.OnWriteCompleted, null);
        }

        /// <summary>
        /// Called when a read in completed.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void OnWriteCompleted(IAsyncResult asyncResult)
        {
            this.stream?.EndWrite(asyncResult);
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
