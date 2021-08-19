using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    public class Arduino : IArduino
    {
        private System.IO.Ports.SerialPort SerialPort { get; set; }
        private ILogger Logger { get; }

        public Arduino(String commPort, ILogger logger)
        {
            this.Logger = logger;
            Open(commPort);
        }

        public bool IsOpen() =>
            SerialPort?.IsOpen ?? false;

        public void Open(String commPort)
        {
            SerialPort = new System.IO.Ports.SerialPort(commPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            SerialPort.Open();
        }

        public void Close()
        {
            SerialPort?.Close();
            SerialPort = null;
        }

        public async Task ReleaseKey(byte key)
        {
            CancellationToken cancellationToken = new CancellationToken(false);
            try
            {
                await SerialPort.BaseStream.WriteAsync(new byte[] { 0x00, key, 0x00, 0xFF }, 0, 4, cancellationToken)
                    .ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "ReleaseKey:WriteAsync", Logger)).ConfigureAwait(false);

                await SerialPort.BaseStream.FlushAsync(cancellationToken)
                    .ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "ReleaseKey:FlushAsync", Logger)).ConfigureAwait(false);
            }
            catch(TaskCanceledException ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        public async Task DepressKey(byte key)
        {
            CancellationToken cancellationToken = new CancellationToken(false);
            try
            {
                await SerialPort.BaseStream.WriteAsync(new byte[] { 0x00, 0x00, key, 0xFF }, 0, 4, cancellationToken)
                    .ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "DepressKey:WriteAsync", Logger)).ConfigureAwait(false);

                await SerialPort.BaseStream.FlushAsync(cancellationToken)
                    .ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "DepressKey:FlushAsync", Logger)).ConfigureAwait(false);
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        public async Task ReleaseAll()
        {
            if (IsOpen() == false)
                return;

            CancellationToken cancellationToken = new CancellationToken(false);

            try
            {
                await SerialPort.BaseStream.WriteAsync(new byte[] { 0x00, 0x00, 0x00, 0xFF }, 0, 4, cancellationToken)
                    .ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "ReleaseAll:WriteAsync", Logger)).ConfigureAwait(false);

                await SerialPort.BaseStream.FlushAsync(cancellationToken)
                    .ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "ReleaseAll:FlushAsync", Logger)).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
            }
        }

        public async Task PressKey(byte key, int duration = 30)
        {
            if (IsOpen() == false)
                return;

            await DepressKey(key).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKey:DepressKey", Logger)).ConfigureAwait(false);
            await Task.Delay(duration).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKey:Delay", Logger)).ConfigureAwait(false);
            await ReleaseKey(key).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKey:ReleaseKey", Logger)).ConfigureAwait(false);
        }

        public async Task TypeFullString(String text) =>
            await Utils.TypeFullString(this, text, Logger).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "TypeFullString", Logger)).ConfigureAwait(false);

        public async Task TypeFromClipboard() =>
            await Utils.TypeFromClipboard(this, Logger).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "TypeFromClipboard", Logger)).ConfigureAwait(false);

        public async Task KeyCombo(byte[] modifier, byte key) =>
            await Utils.KeyCombo(this, modifier, key, Logger).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "KeyCombo", Logger)).ConfigureAwait(false);
    }
}
