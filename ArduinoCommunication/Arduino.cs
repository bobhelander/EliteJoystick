using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    public class Arduino
    {
        private System.IO.Ports.SerialPort serialPort { get; set; }

        public Arduino(String commPort)
        {
            serialPort = new System.IO.Ports.SerialPort(commPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            serialPort.Open();
        }

        public bool IsOpen()
        {
            return serialPort.IsOpen;
        }

        public void ReleaseKey(byte key)
        {
            serialPort.Write(new byte[] { 0x00, key, 0x00, 0xFF}, 0, 4);
        }

        public void DepressKey(byte key)
        {
            serialPort.Write(new byte[] { 0x00, 0x00, key, 0xFF}, 0, 4);
        }

        public void ReleaseAll()
        {
            serialPort.Write(new byte[] { 0x00, 0x00, 0x00, 0xFF }, 0, 4);
        }

        public void PressKey(byte key, int duration = 30)
        {
            DepressKey(key);
            System.Threading.Thread.Sleep(duration);
            ReleaseKey(key);
        }
    }
}
