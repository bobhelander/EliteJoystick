using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleLandingGearCommand : StateHandler
    {
        static UInt32 rightThrottleIdle = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button29;

        private TmThrottleController tmThrottleController;

        public TmThrottleController TmThrottleController
        {
            get { return tmThrottleController; }
            set
            {
                tmThrottleController = value;
                if (null != tmThrottleController)
                {
                    tmThrottleController.Controller.SwitchState += async (s, e) =>
                        await Task.Run(() => ControllerSwitchState(s, e));
                }
            }
        }

        private void ControllerSwitchState(object sender, Faz.SideWinderSC.Logic.TmThrottleSwitchEventArgs e)
        {
            if (TmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, rightThrottleIdle))
            {
                tmThrottleController.SharedState.GearDeployed = true;
            }

            if (TmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, rightThrottleIdle))
            {
                tmThrottleController.SharedState.GearDeployed = false;
            }
        }
    }
}
