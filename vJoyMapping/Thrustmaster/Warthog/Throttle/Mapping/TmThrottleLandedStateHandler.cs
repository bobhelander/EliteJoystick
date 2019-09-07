using EliteJoystick.Common;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static UInt32 leftThrottleParked = (UInt32)Button.Button30;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, leftThrottleParked))
            {
                // Set the left throttle to the middle so we don't move forward or backward when the throttle is parked
                controller.SharedState.LeftThrottleEnabled = false;
                controller.SetJoystickAxis(16 * 1024, HID_USAGES.HID_USAGE_RZ, vJoyTypes.Throttle);
                log.Debug($"Left Throttle Parked");
            }
            else if (Reactive.ButtonReleased(value, leftThrottleParked))
            {
                // Left throttle is set to move forward and backward from the center 
                controller.SharedState.LeftThrottleEnabled = true;
                controller.SetJoystickAxis(32 * 1024, HID_USAGES.HID_USAGE_RZ, vJoyTypes.Throttle);
                log.Debug($"Left Throttle Activated");
            }
        }
    }
}
