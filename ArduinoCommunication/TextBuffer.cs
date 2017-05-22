using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    public class TextBuffer
    {
        public Timer Timer { get; set; }
        public int Delay { get; set; }
        public String Text { get; set; }
        public bool Newline { get; set; }
        public Arduino Arduino { get; set; }
        public EventWaitHandle FinishedEvent { get; set; }

        private int index = 0;
        private bool pressed = false;

         public void Start()
        {
            Timer = new Timer(Action, null, 0, Delay);
        }

        private bool KeyAction(byte key)
        {
            bool finished = false;
            if (!pressed)
                Arduino?.DepressKey(key);
            else
            {
                Arduino?.ReleaseKey(key);
                finished = true;
            }
            pressed = !pressed;
            return finished;
        }

        public void Action(object o)
        {
            if (index < Text.Length)
            {
                if (KeyAction((byte)Text[index]))
                    index++;
            }
            else
            {
                if (Newline)
                {
                    if (KeyAction(0xB0))
                        Newline = false;
                }
                else
                {
                    Timer.Dispose();
                    FinishedEvent?.Set();
                    return;
                }
            }

            Timer.Change(Delay, Timeout.Infinite);
        }
    }
}
