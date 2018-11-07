using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
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
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (null != controller.SharedState)
            {
                // If either state of the Autopilot switch is released 
                if (controller.TestMultiSwitchStateOff(previous.buttons, current.buttons, apahButton))
                    controller.SharedState.ChangeMode(EliteSharedState.Mode.Travel);

                if (controller.TestButtonPressed(previous.buttons, current.buttons, Button27))
                    controller.SharedState.ChangeMode(EliteSharedState.Mode.Fighting);

                if (controller.TestButtonPressed(previous.buttons, current.buttons, Button28))
                    controller.SharedState.ChangeMode(EliteSharedState.Mode.Mining);

                if (controller.TestButtonPressed(previous.buttons, current.buttons, Button01))
                    controller.SharedState.ThrottleShift1 = true;

                if (controller.TestButtonReleased(previous.buttons, current.buttons, Button01))
                    controller.SharedState.ThrottleShift1 = false;

                if (controller.TestButtonReleased(previous.buttons, current.buttons, Button15))
                    controller.SharedState.ThrottleShift2 = true;

                if (controller.TestButtonReleased(previous.buttons, current.buttons, Button15))
                    controller.SharedState.ThrottleShift2 = false;
            }
        }
    }
}
