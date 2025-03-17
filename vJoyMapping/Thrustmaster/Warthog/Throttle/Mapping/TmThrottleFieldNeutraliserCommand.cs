using EliteJoystick.Common;
using System;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleFieldNeutraliserCommand
    {
        private const UInt32 SpeedbrakeForward = (UInt32)Button.Button07;

        public static void Process(States value, Controller controller)
        {
            // Use the speedbrake to lock the FieldNeutraliser button down.  

            if (Reactive.ButtonDown(value, SpeedbrakeForward) &&
                controller.SharedState.CurrentMode == EliteSharedState.Mode.Fighting)
            {
                controller.SetJoystickButton(true, MappedButtons.FieldNeutraliser, vJoyTypes.Virtual);
            }
            else if (controller.SharedState.SecondaryFireActive)
            {
                controller.SetJoystickButton(false, MappedButtons.FieldNeutraliser, vJoyTypes.Virtual);
            }
        }
    }
}
