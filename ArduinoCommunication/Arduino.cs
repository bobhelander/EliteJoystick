using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    public class Arduino
    {
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
    }
}
