using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    public class SendText
    {
        public int Delay { get; set; }
        public String Text { get; set; }
        public bool Newline { get; set; }
        public Arduino Arduino { get; set; }

        public Task Play() =>
            Task.Run(async () => await SendKeys(Arduino, Text, Delay, Newline).ConfigureAwait(false));

        private async Task SendKeys(Arduino arduino, string text, int delay, bool newline)
        {
            foreach (var character in text)
                await arduino?.PressKey((byte)character, delay);

            if (newline)
                await arduino?.PressKey(0xB0, delay);
        }
    }
}
