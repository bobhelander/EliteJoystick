using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common.Logic
{
    public class SendText
    {
        public int Delay { get; set; }
        public String Text { get; set; }
        public bool Newline { get; set; }
        public IKeyboard Keyboard { get; set; }

        public async Task Play(ILogger logger)
        {
            await SendKeys(Keyboard, Text, Delay, Newline, logger)
                .ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "SendText:Play", logger)).ConfigureAwait(false);
        }

        private async Task SendKeys(IKeyboard keyboard, string text, int delay, bool newline, ILogger logger)
        {
            if (keyboard == null)
                return;

            foreach (var character in text)
            {
                await keyboard.PressKey(character.ToString(), delay)
                    .ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "SendKeys:Character", logger)).ConfigureAwait(false);
            }

            if (newline)
            {
                await keyboard.PressKey(0xB0, delay)
                    .ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "SendKeys:Newline", logger)).ConfigureAwait(false);
            }
        }
    }
}
