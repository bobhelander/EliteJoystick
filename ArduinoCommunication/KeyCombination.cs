using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoCommunication
{
    public class KeyCombination
    {
        public int Delay { get; set; }
        public byte[] Modifier { get; set; } = new byte[0];
        public byte Key { get; set; }
        public Arduino Arduino { get; set; }

        // Press down modifier then key.  Release key then release modifier
        public Task Play() =>
            Task.Run(         async () => await PressKeys(Arduino, Modifier, true).ConfigureAwait(false))
                .ContinueWith(async (_) => await Task.Delay(Delay).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await PressKeys(Arduino, new byte[] { Key }, true).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await Task.Delay(Delay).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await PressKeys(Arduino, new byte[] { Key }, false).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await Task.Delay(Delay).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(async (_) => await PressKeys(Arduino, Modifier, false).ConfigureAwait(false), TaskContinuationOptions.OnlyOnRanToCompletion);
        private static async Task PressKeys(Arduino arduino, byte[] keys, bool pressKeys)
        {
            foreach (var key in keys)
            {
                if (pressKeys)
                    await arduino?.DepressKey(key);
                else
                    await arduino?.ReleaseKey(key);
            }
        }
    }
}
