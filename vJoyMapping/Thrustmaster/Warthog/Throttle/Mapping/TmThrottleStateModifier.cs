using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleStateModifier : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        // APAH is between 27 and 28.  It is enabled when both repote false.
        static readonly UInt32 apahButton = (UInt32)SwitchNeutral.APAH;

        static readonly UInt32 Button27 = (UInt32)Button.Button27;
        static readonly UInt32 Button28 = (UInt32)Button.Button28;
        static readonly UInt32 Button01 = (UInt32)Button.Button01;
        static readonly UInt32 Button15 = (UInt32)Button.Button15;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (null != Controller.SharedState)
            {
                // If either state of the Autopilot switch is released 
                if (Controller.TestMultiSwitchStateOff(previous.buttons, current.buttons, apahButton))
                    Controller.SharedState.ChangeMode(EliteSharedState.Mode.Travel);

                if (Controller.TestButtonPressed(previous.buttons, current.buttons, Button27))
                    Controller.SharedState.ChangeMode(EliteSharedState.Mode.Fighting);

                if (Controller.TestButtonPressed(previous.buttons, current.buttons, Button28))
                    Controller.SharedState.ChangeMode(EliteSharedState.Mode.Mining);

                if (Controller.TestButtonPressed(previous.buttons, current.buttons, Button01))
                    Controller.SharedState.ThrottleShift1 = true;

                if (Controller.TestButtonReleased(previous.buttons, current.buttons, Button01))
                    Controller.SharedState.ThrottleShift1 = false;

                if (Controller.TestButtonReleased(previous.buttons, current.buttons, Button15))
                    Controller.SharedState.ThrottleShift2 = true;

                if (Controller.TestButtonReleased(previous.buttons, current.buttons, Button15))
                    Controller.SharedState.ThrottleShift2 = false;
            }
        }
    }
}
