using EliteJoystick.Common.Logic;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace vKeyboard
{
    public partial class Keyboard : IKeyboard, IDisposable
    {
        private ILogger<Keyboard> log { get; set; }

        public Keyboard(
            ILogger<Keyboard> log)
        {
            this.log = log;

            //InitializeDevice(); // Use my code instead
            InitializeController();
            StartKeyboardCommunication();
        }

        public void Dispose()
        {
            DisposeCommunication();
            DisposeController();
            DisposeDevice();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Use keycode to press or release a key.  Adds or removes the key codes to the current keys buffer.
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="code"></param>
        /// <param name="pressed"></param>
        /// <returns></returns>
        private async Task KeyAction(byte modifier, byte code, bool pressed) =>
            await Key(modifier, code, pressed).ConfigureAwait(false);

        private async Task PressKey(byte modifiers, byte code, int duration = 50)
        {
            await KeyAction(modifiers, code, true).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKey:DepressKey", log)).ConfigureAwait(false);
            await Task.Delay(duration).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKey:Delay", log)).ConfigureAwait(false);
            await KeyAction(modifiers, code, false).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKey:ReleaseKey", log)).ConfigureAwait(false);
            await Task.Delay(duration).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "After ReleaseKey:Delay", log)).ConfigureAwait(false);
        }

        private static (byte modifier, byte code) FindCode(string key)
        {
            if (KeyMap.LowerCaseMap.ContainsKey(key))
                return (0x00, KeyMap.LowerCaseMap[key].Code);
            if (KeyMap.UpperCaseMap.ContainsKey(key))
                return (KeyMap.ModifierKeys[1].Code, KeyMap.UpperCaseMap[key].Code);

            return (0x00, 0x00);
        }

        /// <summary>
        /// Add the modifiers together, if any.
        /// </summary>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        private static byte CombineModifiers(KeyCode[] modifiers) =>
            modifiers?.Aggregate((keyCode, modifier) => keyCode.Combine(modifier)).Code ?? (byte)0;

        #region IKeyboard
        public async Task PressKey(byte code, KeyCode[] modifiers = null, int duration = 50)
        {
            byte modiferKeys = CombineModifiers(modifiers);

            if (duration == -1) // Press the combination
                await KeyAction(modiferKeys, code, true).ConfigureAwait(false);
            else // Press and release
                await PressKey(modiferKeys, code, duration).ConfigureAwait(false);
        }

        public async Task ReleaseKey(byte code, KeyCode[] modifiers = null)
        {
            byte modiferKeys = CombineModifiers(modifiers);
            await KeyAction(modiferKeys, code, false).ConfigureAwait(false);
        }

        public Task ReleaseAll()
        {
            Release(true, true);
            return Task.CompletedTask;
        }

        public async Task PressKey(string value, KeyCode[] modifiers = null, int duration = 50)
        {
            (var modifier, var code) = FindCode(value);
            byte modiferKeys = (byte)(CombineModifiers(modifiers) | modifier);

            if (duration == -1) // Press the combination
                await KeyAction(modiferKeys, code, true).ConfigureAwait(false);
            else // Press and release
                await PressKey(modiferKeys, code, duration).ConfigureAwait(false);
        }

        public async Task ReleaseKey(string value, KeyCode[] modifiers = null)
        {
            (var modifier, var code) = FindCode(value);
            byte modiferKeys = (byte)(CombineModifiers(modifiers) | modifier);
            await KeyAction(modiferKeys, code, false).ConfigureAwait(false);
        }

        public async Task TypeFullString(String text) =>
            await Utils.TypeFullString(this, text, log).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "TypeFullString", log)).ConfigureAwait(false);

        public async Task TypeFromClipboard() =>
            await Utils.TypeFromClipboard(this, log).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "TypeFromClipboard", log)).ConfigureAwait(false);
        #endregion
    }
}
