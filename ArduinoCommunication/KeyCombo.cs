using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    public class KeyCombo
    {
        public Timer Timer { get; set; }
        public int Delay { get; set; }
        public byte[] Modifier { get; set; }
        public byte Key { get; set; }
        public Arduino Arduino { get; set; }

        private bool stepOne = false;
        private bool stepTwo = false;
        private bool stepThree = false;

        public void Start()
        {
            Timer = new Timer(Action, null, 0, Delay);
        }

        public void Action(object o)
        {
            if (false == stepOne)
            {
                foreach (byte modifier in Modifier)
                    Arduino?.DepressKey(modifier);
                stepOne = true;
            }
            else if (false == stepTwo)
            {
                Arduino?.DepressKey(Key);
                stepTwo = true;
            }
            else if (false == stepThree)
            {
                Arduino?.ReleaseKey(Key);
                stepThree = true;
            }
            else
            {
                foreach (byte modifier in Modifier)
                    Arduino?.ReleaseKey(modifier);
                Timer.Dispose();
                return;
            }

            Timer.Change(Delay, Timeout.Infinite);
        }
    }
}
