using EliteJoystick.Common;
using EliteJoystick.Common.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping
{
    public static class SwGameMuteHandler
    {
        private const byte muteButton = (byte)Button.MuteButton;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, muteButton))
            {
                // Mute EDDI
                //Task.Run(async () => await controller.SendKeyCombo(new byte[] { }, 0xCC).ConfigureAwait(false));  // KEY_F11
                Task.Run(async () => await controller.PressKey(0x00, KeyMap.KeyNameMap["KEY_F11"].Code).ConfigureAwait(false));
            }
            else if (Reactive.ButtonReleased(value, muteButton))
            {
                // Unmute EDDI
                //Task.Run(async () => await controller.SendKeyCombo(new byte[] { }, 0xCD).ConfigureAwait(false));  // KEY_F12
                Task.Run(async () => await controller.PressKey(0x00, KeyMap.KeyNameMap["KEY_F12"].Code).ConfigureAwait(false));
            }
        }
    }
}
