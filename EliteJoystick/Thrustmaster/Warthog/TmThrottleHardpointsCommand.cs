using EliteJoystick.Thrustmaster.Warthog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystick
{
    public class TmThrottleHardpointsCommand : StateHandler
    {
        public Faz.SideWinderSC.Logic.TmThrottleButton ButtonId { get; set; }

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
            var button19 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button19;

            if (tmThrottleController.SharedState.HardpointsDeployed == false &&
                tmThrottleController.TestButtonChanged(e.PreviousButtons, e.Buttons, button19))
            {
                tmThrottleController.SharedState.HardpointsDeployed = true;
                tmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.HardpointsToggle, 200);
                tmThrottleController.VisualState.UpdateMessage("Hardpoints Deployed");
            }

            if (tmThrottleController.SharedState.HardpointsDeployed == true &&
                tmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, button19))
            {
                tmThrottleController.SharedState.HardpointsDeployed = true;
                tmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.HardpointsToggle, 200);
                tmThrottleController.VisualState.UpdateMessage("Hardpoints Deployed");
            }
        }
    }
}
