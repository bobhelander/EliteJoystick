using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vKeyboard
{
    public partial class Keyboard
    {
        private static readonly IObservable<int> timer = Observable.Interval(TimeSpan.FromMilliseconds(25)).Select(_ => 1);
        private static IDisposable communicationLoop = null;

        private static bool KeysCleared = false;

        private byte modifiers { get; set; }
        private HashSet<byte> keys { get; } = new HashSet<byte>();

        private void Key(byte key, bool pressed)
        {
            lock (keys)
            {
                if (pressed) keys.Add(key);
                else keys.Remove(key);
            }
        }

        private void Modifier(byte key, bool pressed)
        {
            if (pressed) modifiers |= key;
            else modifiers &= (byte)~key;
        }

        private void Release(bool modifiers, bool keys)
        {
            if (keys) 
            {
                lock (this.keys)
                {
                    this.keys.Clear();
                }
            }
            if (modifiers)
                this.modifiers = 0;
        }

        private void StartKeyboardCommunication()
        {
            communicationLoop = timer.Subscribe(_ => SendMessage().Wait());
        }

        private bool SendUpdate()
        {
            if (modifiers != 0)
            {
                KeysCleared = false;
                return true;
            }

            lock (keys)
            {
                if (keys.Any(x => x != 0))
                {
                    KeysCleared = false;
                    return true;
                }
            }

            if (KeysCleared == false)
            {
                KeysCleared = true;
                return true;
            }

            return false;
        }

        private async Task SendMessage()
        {
            if (SendUpdate() == false) return;

            (var code0, var code1, var code2, var code3, var code4, var code5) = GetPressedKeys();

            log.LogDebug($"Keyboard buffer: 0x{modifiers:X}, 0x{code0:X}, 0x{code1:X}, 0x{code2:X}, 0x{code3:X}, 0x{code4:X}, 0x{code5:X}");

            await SendAsync(modifiers, 0x00, code0, code1, code2, code3, code4, code5).ConfigureAwait(false);
        }

        private (byte, byte, byte, byte, byte, byte) GetPressedKeys()
        {
            lock (keys)
            {
                var pressedKeys = keys.ToArray();
                // Determine what keys are pressed
                return (
                    GetKeyPressed(pressedKeys, 0),
                    GetKeyPressed(pressedKeys, 1),
                    GetKeyPressed(pressedKeys, 2),
                    GetKeyPressed(pressedKeys, 3),
                    GetKeyPressed(pressedKeys, 4),
                    GetKeyPressed(pressedKeys, 5)
               );
            }
        }

        private static byte GetKeyPressed(byte[] keys, int index) =>
             (keys.Length > index) ? keys[index] : (byte)0x00;

        private void DisposeCommunication()
        {
            communicationLoop.Dispose();
            communicationLoop = null;
        }
    }
}
