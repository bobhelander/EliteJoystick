using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleLightsCommand
    {
        static readonly UInt32 FuelLeft = (UInt32)Button.Button16;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (controller.TestButtonPressedOrReleased(previous.buttons, current.buttons, FuelLeft))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LightsToggle, 200);
            }
        }
    }
}
