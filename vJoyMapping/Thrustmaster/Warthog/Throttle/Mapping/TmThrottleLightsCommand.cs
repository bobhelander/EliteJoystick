using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleLightsCommand
    {
        static readonly UInt32 FuelLeft = (UInt32)Button.Button16;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressedOrReleased(value, FuelLeft))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LightsToggle, 200);
            }
        }
    }
}
