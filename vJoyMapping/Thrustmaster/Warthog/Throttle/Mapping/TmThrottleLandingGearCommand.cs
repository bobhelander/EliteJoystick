using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleLandingGearCommand
    {
        static UInt32 rightThrottleIdle = (UInt32)Button.Button29;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, rightThrottleIdle))
            {
                controller.SharedState.ChangeGear(true);
            }

            if (Reactive.ButtonReleased(value, rightThrottleIdle))
            {
                controller.SharedState.ChangeGear(false);
            }
        }
    }
}
