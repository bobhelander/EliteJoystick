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
        }

        public void Dispose()
        {
            DisposeController();
            DisposeDevice();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public Task DepressKey(byte code) => KeyAction(0x00, code, true);
        public Task DepressKey(string key) => KeyAction(key, true);

        public Task ReleaseKey(byte code) => KeyAction(0x00, code, false);
        public Task ReleaseKey(string key) => KeyAction(key, false);

        public Task ReleaseAll() => KeyAction(0x00, 0x00, false);

        public async Task KeyAction(byte modifiers, byte code0, byte code1 = 0, byte code2 = 0, byte code3 = 0, byte code4 = 0, byte code5 = 0)
            => await SendAsync(modifiers, 0x00, code0, code1, code2, code3, code4, code5).ConfigureAwait(false);

        private async Task KeyAction(byte modifier, byte code, bool pressed) =>
            await Key(modifier, code, pressed).ConfigureAwait(false);

        private async Task KeyAction(string key, bool pressed)
        {
            (var modifier, var code) = FindCode(key);
            await KeyAction(modifier, code, pressed).ConfigureAwait(false);
        }

        public async Task TypeFromClipboard() =>
            await Utils.TypeFromClipboard(this, log).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "TypeFromClipboard", log)).ConfigureAwait(false);

        public async Task PressKey(string value, int duration = 30)
        {
            (var modifier, var code) = FindCode(value);
            await PressKey(modifier, code, duration).ConfigureAwait(false);
        }

        public async Task PressKey(byte code, int duration = 50) =>
            await PressKey(0x00, code, duration).ConfigureAwait(false);

        public async Task PressKey(byte modifiers, byte code, int duration = 50)
        {
            await KeyAction(modifiers, code).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKey:DepressKey", log)).ConfigureAwait(false);
            await Task.Delay(duration).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKey:Delay", log)).ConfigureAwait(false);
            await KeyAction(0x00, 0x00).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "PressKey:ReleaseKey", log)).ConfigureAwait(false);
            await Task.Delay(duration).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "After ReleaseKey:Delay", log)).ConfigureAwait(false);
        }

        public async Task TypeFullString(String text) =>
            await Utils.TypeFullString(this, text, log).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "TypeFullString", log)).ConfigureAwait(false);

        public async Task KeyCombo(byte[] modifier, byte key) =>
            await Utils.KeyCombo(this, modifier, key, log).ContinueWith(t => EliteJoystick.Common.Utils.LogTaskResult(t, "KeyCombo", log)).ConfigureAwait(false);

        private static (byte modifier, byte code) FindCode(string key)
        {
            if (KeyMap.LowerCaseMap.ContainsKey(key))
                return (0x00, KeyMap.LowerCaseMap[key].Code);
            if (KeyMap.UpperCaseMap.ContainsKey(key))
                return (KeyMap.ModifierKeys[1].Code, KeyMap.UpperCaseMap[key].Code);

            return (0x00, 0x00);
        }
    }
}
