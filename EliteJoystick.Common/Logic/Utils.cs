using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using EliteJoystick.Common.Interfaces;

namespace EliteJoystick.Common.Logic
{
    public static class Utils
    {
        public static async Task TypeFromClipboard(IKeyboard keyboard, ILogger logger)
        {
            var text = await Utils.CallClipboard().ConfigureAwait(false);
            await keyboard.TypeFullString(text).ConfigureAwait(false);
        }

        public static Task TypeFullString(IKeyboard keyboard, String text, ILogger logger)
        {
            return new SendText
            {
                Delay = 40,
                Text = text,
                Keyboard = keyboard,
                Newline = true,
            }.Play(logger);
        }

        /*
        public static Task KeyCombo(IKeyboard keyboard, byte[] modifier, byte key, ILogger logger)
        {
            return new KeyCombination
            {
                Delay = 150,
                Modifier = modifier,
                Key = key,
                Keyboard = keyboard
            }.Play(logger);
        }
        */

        static public async Task<string> CallClipboard() =>
            await StartSTATask<string>(() => Clipboard.GetText()).ConfigureAwait(false);

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
