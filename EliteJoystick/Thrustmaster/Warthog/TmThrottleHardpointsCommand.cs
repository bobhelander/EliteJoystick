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
        static UInt32 button19 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button19;

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
            if (tmThrottleController.SharedState.HardpointsDeployed == false &&
                tmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, button19))
            {
                tmThrottleController.SharedState.HardpointsDeployed = true;
                tmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.HardpointsToggle, 200);
                tmThrottleController.VisualState.UpdateMessage($"Hardpoints Deployed {e.Buttons} {e.PreviousButtons}");
            }

            if (tmThrottleController.SharedState.HardpointsDeployed == true &&
                tmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, button19))
            {
                tmThrottleController.SharedState.HardpointsDeployed = false;
                tmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.HardpointsToggle, 200);
                tmThrottleController.VisualState.UpdateMessage($"Hardpoints Retracted  {e.Buttons} {e.PreviousButtons}");
            }
        }
    }
}
