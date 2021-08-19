using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping
{
    public static class SwGameLandingGearHandler
    {
        static private byte button1 = (byte)Button.Button1;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, button1))
            {
                // Deployed
                controller.SharedState.ChangeGear(true);
            }
            else if (Reactive.ButtonReleased(value, button1))
            {
                // Retracted
                controller.SharedState.ChangeGear(false);
            }
        }
    }
}
