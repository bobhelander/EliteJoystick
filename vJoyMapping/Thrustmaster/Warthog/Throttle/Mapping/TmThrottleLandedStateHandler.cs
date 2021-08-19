using EliteJoystick.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleLandedStateHandler
    {
        private const UInt32 leftThrottleParked = (UInt32)Button.Button30;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, leftThrottleParked))
            {
                // Set the left throttle to the middle so we don't move forward or backward when the throttle is parked
                controller.SharedState.LeftThrottleEnabled = false;
                controller.SetJoystickAxis(16 * 1024, vJoy.Wrapper.Axis.HID_USAGE_RZ, vJoyTypes.Throttle);
                controller.Logger.LogDebug("Left Throttle Parked");
            }
            else if (Reactive.ButtonReleased(value, leftThrottleParked))
            {
                // Left throttle is set to move forward and backward from the center 
                controller.SharedState.LeftThrottleEnabled = true;
                controller.SetJoystickAxis(32 * 1024, vJoy.Wrapper.Axis.HID_USAGE_RZ, vJoyTypes.Throttle);
                controller.Logger.LogDebug("Left Throttle Activated");
            }
        }
    }
}
