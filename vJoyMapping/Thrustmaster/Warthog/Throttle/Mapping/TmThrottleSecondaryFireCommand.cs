using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleSecondaryFireCommand : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        static readonly UInt32 SpeedbrakeForward = (UInt32)Button.Button07;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            // Use the speedbrake to lock the secondary fire button down.  
            // This will hold the mining laser or discovery scanner on until the switch is moved off.
            // In fighting mode this switch is used to cycle the subsystem targeting.

            if (Controller.TestButtonDown(current.buttons, SpeedbrakeForward) &&
               (Controller.SharedState.CurrentMode == EliteSharedState.Mode.Travel ||
                Controller.SharedState.CurrentMode == EliteSharedState.Mode.Mining))
            {
                Controller.SharedState.SecondaryFireActive = true;
                Controller.SetJoystickButton(true, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
            else if (Controller.SharedState.SecondaryFireActive)
            {
                Controller.SharedState.SecondaryFireActive = false;
                Controller.SetJoystickButton(false, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
        }
    }
}
