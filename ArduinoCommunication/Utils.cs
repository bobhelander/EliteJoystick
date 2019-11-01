using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ArduinoCommunication
{
    public static class Utils
    {
        public static async Task TypeFromClipboard(Arduino arduino)
        {
            var text = await Utils.CallClipboard().ConfigureAwait(false);
            await TypeFullString(arduino, text).ConfigureAwait(false);
        }

        public static Task TypeFullString(Arduino arduino, String text)
        {
            return new SendText
            {
                Delay = 40,
                Text = text,
                Arduino = arduino,
                Newline = true,
            }.Play();
        }

        public static Task KeyCombo(Arduino arduino, byte[] modifier, byte key)
        {
            return new KeyCombination
            {
                Delay = 150,
                Modifier = modifier,
                Key = key,
                Arduino = arduino
            }.Play();
        }

        static public async Task<string> CallClipboard() =>
            await StartSTATask<string>(() => System.Windows.Clipboard.GetText()).ConfigureAwait(false);

        public static Task<T> StartSTATask<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            Thread thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }
    }
}
