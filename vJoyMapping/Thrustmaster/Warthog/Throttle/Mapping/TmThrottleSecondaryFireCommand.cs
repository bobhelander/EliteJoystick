using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleSecondaryFireCommand
    {
        static readonly UInt32 SpeedbrakeForward = (UInt32)Button.Button07;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            // Use the speedbrake to lock the secondary fire button down.  
            // This will hold the mining laser or discovery scanner on until the switch is moved off.
            // In fighting mode this switch is used to cycle the subsystem targeting.

            if (controller.TestButtonDown(current.buttons, SpeedbrakeForward) &&
               (controller.SharedState.CurrentMode == EliteSharedState.Mode.Travel ||
                controller.SharedState.CurrentMode == EliteSharedState.Mode.Mining))
            {
                controller.SharedState.SecondaryFireActive = true;
                controller.SetJoystickButton(true, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
            else if (controller.SharedState.SecondaryFireActive)
            {
                controller.SharedState.SecondaryFireActive = false;
                controller.SetJoystickButton(false, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
        }
    }
}
