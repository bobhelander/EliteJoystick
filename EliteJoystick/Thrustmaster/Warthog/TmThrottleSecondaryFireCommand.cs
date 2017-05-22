using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Thrustmaster.Warthog
{
    public class TmThrottleSecondaryFireCommand : StateHandler
    {
        public uint vButton { get; set; }

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
            if ((e.Buttons & (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button07) != 0 &&
               (tmThrottleController.SharedState.CurrentMode == EliteSharedState.Mode.Travel ||
                tmThrottleController.SharedState.CurrentMode == EliteSharedState.Mode.Mining))
            {
                tmThrottleController.SetJoystickButton(true, vButton, vJoyTypes.Virtual);
                tmThrottleController.VisualState.UpdateMessage("Secondary Fire Activated");
            }
            else
            {
                tmThrottleController.SetJoystickButton(false, vButton, vJoyTypes.Virtual);
            }
        }
    }
}
