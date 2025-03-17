using EliteJoystick.Common.Logic;
using System;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;

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
                // Oculus ASW off  CRTL+KP1  // Remapped to Midi controller
                // Task.Run(async () => await controller.PressKey(KeyMap.KeyNameMap["KEY_J"].Code).ConfigureAwait(false));
            }
            if (Reactive.ButtonPressed(value, Button7))
            {
                // Take Picture  ALT-F10
                controller.PressKey(KeyMap.KeyNameMap["KEY_F10"].Code,
                    new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LALT"] });
            }
            if (Reactive.ButtonPressed(value, Button5))
            {
                // HUD off  CRTL+ALT+G
                controller.PressKey(
                    KeyMap.KeyNameMap["KEY_G"].Code,  // G
                    new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"], KeyMap.ModifierKeyNameMap["KEY_MOD_LALT"] }); // CTRL+ALT
            }
        }
    }
}
