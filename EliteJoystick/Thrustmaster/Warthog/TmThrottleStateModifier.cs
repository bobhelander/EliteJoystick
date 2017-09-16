using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Thrustmaster.Warthog
{
    public class TmThrottleStateModifier : StateHandler
    {
        // APAH is between 27 and 28.  It is enabled when both repote false.
        static UInt32 apahButton = (UInt32)Faz.SideWinderSC.Logic.TmThrottleSwitchNeutral.APAH;

        static UInt32 Button27 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button27;
        static UInt32 Button28 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button28;
        static UInt32 Button01 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button01;

        private TmThrottleController tmThrottleController;

        public TmThrottleController TmThrottleController
        {
            get { return tmThrottleController; }
            set
            {
                tmThrottleController = value;
                if (null != tmThrottleController)
                {
                    tmThrottleController.Controller.SwitchState += Controller_SwitchState;
                }
            }
        }

        private void Controller_SwitchState(object sender, Faz.SideWinderSC.Logic.TmThrottleSwitchEventArgs e)
        {
            if (null != TmThrottleController.SharedState)
            {
                // If either state of the Autopilot switch is released 
                if (TmThrottleController.TestMultiSwitchStateOff(e.PreviousButtons, e.Buttons, apahButton))
                    TmThrottleController.SharedState.CurrentMode = EliteSharedState.Mode.Travel;

                if (TmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, Button27))
                    TmThrottleController.SharedState.CurrentMode = EliteSharedState.Mode.Fighting;

                if (TmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, Button28))
                    TmThrottleController.SharedState.CurrentMode = EliteSharedState.Mode.Mining;

                if (TmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, Button01))
                    TmThrottleController.SharedState.ThrottleShiftStateValue = EliteSharedState.ThrottleShiftState.Shift1;

                if (TmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, Button01))
                    TmThrottleController.SharedState.ThrottleShiftStateValue = EliteSharedState.ThrottleShiftState.None;
            }
        }
    }
}
