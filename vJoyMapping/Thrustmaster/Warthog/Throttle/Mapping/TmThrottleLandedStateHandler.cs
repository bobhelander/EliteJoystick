using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleLandedStateHandler : IObserver<States>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        static UInt32 leftThrottleParked = (UInt32)Button.Button30;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (Controller.TestButtonPressed(previous.buttons, current.buttons, leftThrottleParked))
            {
                // Set the left throttle to the middle so we don't move forward or backward when the throttle is parked
                Controller.SharedState.LeftThrottleEnabled = false;
                Controller.SetJoystickAxis(16*1024, HID_USAGES.HID_USAGE_RZ, vJoyTypes.Throttle);
                log.Debug($"Left Throttle Parked");
            }
            else if (Controller.TestButtonReleased(previous.buttons, current.buttons, leftThrottleParked))
            {
                // Left throttle is set to move forward and backward from the center 
                Controller.SharedState.LeftThrottleEnabled = true;
                Controller.SetJoystickAxis(32*1024, HID_USAGES.HID_USAGE_RZ, vJoyTypes.Throttle);
                log.Debug($"Left Throttle Activated");
            }
        }
    }
}
