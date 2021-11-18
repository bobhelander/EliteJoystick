using EliteJoystick.Common;
using EliteJoystick.Common.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2UtilCommandsStateHandler
    {
        private const UInt32 Button8 = (UInt32)Button.Button8;
        private const UInt32 Button7 = (UInt32)Button.Button7;
        private const UInt32 Button6 = (UInt32)Button.Button6;
        private const UInt32 Button5 = (UInt32)Button.Button5;
        private const UInt32 Button2 = (UInt32)Button.Button2;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, Button8))
            {
                // Oculus ASW off  CRTL+KP1
                //Task.Run(async () => await controller.SendKeyCombo(new byte[] { 0x80 }, 0x31).ConfigureAwait(false));
                Task.Run(async () => await controller.PressKey(0x00, KeyMap.KeyNameMap["KEY_J"].Code).ConfigureAwait(false));
            }
            if (Reactive.ButtonPressed(value, Button7))
            {
                // Take Picture  ALT-F10
                //Task.Run(async () => await controller.SendKeyCombo(new byte[] { 0x82 }, 0xCB).ConfigureAwait(false));
                Task.Run(async () => await controller.PressKey(KeyMap.ModifierKeyNameMap["KEY_MOD_LALT"].Code, KeyMap.KeyNameMap["KEY_F10"].Code).ConfigureAwait(false));
            }
            if (Reactive.ButtonPressed(value, Button5))
            {
                // HUD off  CRTL+ALT+G
                //Task.Run(async () => await controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47).ConfigureAwait(false));
                Task.Run(async () => await controller.PressKey(
                    (byte)(KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"].Code | KeyMap.ModifierKeyNameMap["KEY_MOD_LALT"].Code),
                    KeyMap.KeyNameMap["KEY_G"].Code).ConfigureAwait(false));
            }
        }
    }
}
