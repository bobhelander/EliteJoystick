using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping
{
    public static class SwGameMuteHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static private byte muteButton = (byte)Button.MuteButton;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, muteButton))
            {
                // Mute EDDI
                controller.SendKeyCombo(new byte[] { }, 0xCC);  // KEY_F11
            }
            else if (Reactive.ButtonReleased(value, muteButton))
            {
                // Unmute EDDI
                controller.SendKeyCombo(new byte[] { }, 0xCD);  // KEY_F12
            }
        }
    }
}
