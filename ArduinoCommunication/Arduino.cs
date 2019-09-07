using EliteJoystick.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    public class Arduino : IArduino
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private System.IO.Ports.SerialPort SerialPort { get; set; }

        public Arduino(String commPort)
        {
            Open(commPort);
        }

        public bool IsOpen()
        {
            return SerialPort?.IsOpen ?? false;
        }

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
            await SerialPort.BaseStream.WriteAsync(new byte[] { 0x00, key, 0x00, 0xFF }, 0, 4).ConfigureAwait(false);
        }

        public async Task DepressKey(byte key)
        {
            await SerialPort.BaseStream.WriteAsync(new byte[] { 0x00, 0x00, key, 0xFF }, 0, 4).ConfigureAwait(false);
        }

        public async Task ReleaseAll()
        {
            await SerialPort?.BaseStream.WriteAsync(new byte[] { 0x00, 0x00, 0x00, 0xFF }, 0, 4);
        }

        public async Task PressKey(byte key, int duration = 30)
        {
            await DepressKey(key).ConfigureAwait(false);
            await Task.Delay(duration).ConfigureAwait(false);
            await ReleaseKey(key).ConfigureAwait(false);
        }

        public async Task TypeFullString(String text)
        {
            await Task.Run(async () => await Utils.TypeFullString(this, text).ConfigureAwait(false))
             .ContinueWith(t => log.Error($"SendKeyCombo Exception: {t.Exception}"), TaskContinuationOptions.OnlyOnFaulted).ConfigureAwait(false);
        }

        public async Task TypeFromClipboard()
        {
            await Task.Run(async () => await Utils.TypeFromClipboard(this).ConfigureAwait(false))
             .ContinueWith(t => log.Error($"TypeFromClipboard Exception: {t.Exception}"), TaskContinuationOptions.OnlyOnFaulted).ConfigureAwait(false);
        }

        public async Task KeyCombo(byte[] modifier, byte key)
        {
            await Task.Run(async () => await Utils.KeyCombo(this, modifier, key).ConfigureAwait(false))
             .ContinueWith(t => log.Error($"SendKeyCombo Exception: {t.Exception}"), TaskContinuationOptions.OnlyOnFaulted).ConfigureAwait(false);
        }
    }
}
