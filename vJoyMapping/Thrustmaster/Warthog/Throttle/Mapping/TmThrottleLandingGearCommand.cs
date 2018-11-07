using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleLandingGearCommand : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        static UInt32 rightThrottleIdle = (UInt32)Button.Button29;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (Controller.TestButtonPressed(previous.buttons, current.buttons, rightThrottleIdle))
            {
                Controller.SharedState.ChangeGear(true);
            }

            if (Controller.TestButtonReleased(previous.buttons, current.buttons, rightThrottleIdle))
            {
                Controller.SharedState.ChangeGear(false);
            }
        }
    }
}
