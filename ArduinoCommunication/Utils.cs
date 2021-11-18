using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using EliteJoystick.Common.Interfaces;

namespace ArduinoCommunication
{
    public static class Utils
    {
        public static async Task TypeFromClipboard(IKeyboard arduino, ILogger logger)
        {
            var text = await Utils.CallClipboard().ConfigureAwait(false);
            await TypeFullString(arduino, text, logger).ConfigureAwait(false);
        }

        public static Task TypeFullString(IKeyboard arduino, String text, ILogger logger)
        {
            return new SendText
            {
                Delay = 40,
                Text = text,
                Arduino = arduino,
                Newline = true,
            }.Play(logger);
        }

        public static Task KeyCombo(IKeyboard arduino, byte[] modifier, byte key, ILogger logger)
        {
            return new KeyCombination
            {
                Delay = 150,
                Modifier = modifier,
                Key = key,
                Arduino = arduino
            }.Play(logger);
        }

        //static public async Task<string> CallClipboard() =>
        //    await StartSTATask<string>(() => System.Windows.Clipboard.GetText()).ConfigureAwait(false);

        static public async Task<string> CallClipboard() =>
            await StartSTATask<string>(() => CommonCommunication.Clipboard.GetText()).ConfigureAwait(false);

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
