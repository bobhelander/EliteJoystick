using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleLightsCommand : StateHandler
    {
        // Fuel Left : Lights
        static UInt32 button16 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button16;

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
            if (TmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, button16))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LightsToggle, 200);
                //TmThrottleController.VisualState.UpdateMessage("Lights On");
            }
            if (TmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, button16))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LightsToggle, 200);
                //tmThrottleController.VisualState.UpdateMessage("Lights Off");
            }
        }
    }
}
