using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Thrustmaster.Warthog
{
    public class TmThrottleStateModifier : StateHandler
    {
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
            if (null != tmThrottleController.SharedState)
            {
                if ((e.Buttons & (UInt32)Faz.SideWinderSC.Logic.TmThrottleSwitchNeutral.APAH) == 0)
                    tmThrottleController.SharedState.CurrentMode = EliteSharedState.Mode.Travel;

                if ((e.Buttons & (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button27) != 0)
                    tmThrottleController.SharedState.CurrentMode = EliteSharedState.Mode.Fighting;

                if ((e.Buttons & (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button28) != 0)
                    tmThrottleController.SharedState.CurrentMode = EliteSharedState.Mode.Mining;

                if ((e.Buttons & (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button01) != 0)
                    tmThrottleController.SharedState.ThrottleShiftStateValue = EliteSharedState.ThrottleShiftState.Shift1;

                if ((e.Buttons & (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button01) == 0)
                    tmThrottleController.SharedState.ThrottleShiftStateValue = EliteSharedState.ThrottleShiftState.None;
            }
        }
    }
}
