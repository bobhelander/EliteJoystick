using EliteJoystick.Thrustmaster.Warthog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystick
{
    public class TmThrottleSilentCommand : StateHandler
    {
        // Fuel Right : Silent
        static UInt32 button17 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button17;

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
            if (TmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, button17))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                TmThrottleController.VisualState.UpdateMessage("Silent Running Activated");
            }
            if (TmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, button17))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                tmThrottleController.VisualState.UpdateMessage("Silent Running Deactivated");
            }
        }
    }
}
