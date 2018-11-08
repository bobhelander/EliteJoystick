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
            SerialPort = new System.IO.Ports.SerialPort(commPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            SerialPort.Open();
        }

        public bool IsOpen()
        {
            return SerialPort.IsOpen;
        }

        public async Task ReleaseKey(byte key)
        {
            await SerialPort.BaseStream.WriteAsync(new byte[] { 0x00, key, 0x00, 0xFF}, 0, 4);
        }

        public async Task DepressKey(byte key)
        {
            await SerialPort.BaseStream.WriteAsync(new byte[] { 0x00, 0x00, key, 0xFF}, 0, 4);
        }

        public async Task ReleaseAll()
        {
            await SerialPort.BaseStream.WriteAsync(new byte[] { 0x00, 0x00, 0x00, 0xFF }, 0, 4);
        }

        public async Task PressKey(byte key, int duration = 30)
        {
            await DepressKey(key);
            await Task.Delay(duration);
            await ReleaseKey(key);
        }

        public async Task TypeFullString(String text)
        {
            Task.Run(async () => await ArduinoCommunication.Utils.TypeFullString(this, text))
             .ContinueWith(t => { log.Error($"SendKeyCombo Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public async Task TypeFromClipboard()
        {
            Task.Run(async () => await ArduinoCommunication.Utils.TypeFromClipboard(this))
             .ContinueWith(t => { log.Error($"TypeFromClipboard Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public async Task KeyCombo(byte[] modifier, byte key)
        {
            Task.Run(async () => await ArduinoCommunication.Utils.KeyCombo(this, modifier, key))
             .ContinueWith(t => { log.Error($"SendKeyCombo Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
