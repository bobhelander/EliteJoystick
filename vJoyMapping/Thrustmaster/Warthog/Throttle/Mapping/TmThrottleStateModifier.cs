using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleStateModifier
    {
        // APAH is between 27 and 28.  It is enabled when both repote false.
        static readonly UInt32 apahButton = (UInt32)SwitchNeutral.APAH;

        static readonly UInt32 Button27 = (UInt32)Button.Button27;
        static readonly UInt32 Button28 = (UInt32)Button.Button28;
        static readonly UInt32 Button01 = (UInt32)Button.Button01;
        static readonly UInt32 Button15 = (UInt32)Button.Button15;

        public static void Process(States value, Controller controller)
        {
            if (null != controller.SharedState)
            {
                // If either state of the Autopilot switch is released 
                if (Reactive.MultiSwitchStateOff(value, apahButton))
                    controller.SharedState.ChangeMode(EliteSharedState.Mode.Travel);

                if (Reactive.ButtonPressed(value, Button27))
                    controller.SharedState.ChangeMode(EliteSharedState.Mode.Fighting);

                if (Reactive.ButtonPressed(value, Button28))
                    controller.SharedState.ChangeMode(EliteSharedState.Mode.Mining);

                if (Reactive.ButtonPressed(value, Button01))
                    controller.SharedState.ThrottleShift1 = true;

                if (Reactive.ButtonReleased(value, Button01))
                    controller.SharedState.ThrottleShift1 = false;

                if (Reactive.ButtonReleased(value, Button15))
                    controller.SharedState.ThrottleShift2 = true;
            }
        }
    }
}
