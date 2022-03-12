using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common.Logic
{
    /*
    public class KeyCombination
    {
        public int Delay { get; set; }
        public byte[] Modifier { get; set; } = new byte[0];
        public byte Key { get; set; }
        public IKeyboard Keyboard { get; set; }

        // Press down modifier then key.  Release key then release modifier
        public Task Play(ILogger logger) =>
            Task.Run(         async () => await PressKeys(Keyboard, Modifier, true, logger).ConfigureAwait(false))
                .ContinueWith(async (_) => await Task.Delay(Delay).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await PressKeys(Keyboard, new byte[] { Key }, true, logger).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await Task.Delay(Delay).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await PressKeys(Keyboard, new byte[] { Key }, false, logger).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await Task.Delay(Delay).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await PressKeys(Keyboard, Modifier, false, logger).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion);
        private static async Task PressKeys(IKeyboard arduino, byte[] keys, bool pressKeys, ILogger logger)
        {
            if (arduino == null)
                return;

            foreach (var key in keys)
            {
                if (pressKeys)
                    await arduino.DepressKey(key).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKeys:DepressKey", logger)).ConfigureAwait(false);
                else
                    await arduino.ReleaseKey(key).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKeys:ReleaseKey", logger)).ConfigureAwait(false);
            }
        }
    }
    */
}
