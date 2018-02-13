using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleSecondaryFireCommand : StateHandler
    {
        static UInt32 button07 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button07;

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
            // Use the speedbrake to lock the secondary fire button down.  
            // This will hold the mining laser or discovery scanner on until the switch is moved off.
            // In fighting mode this switch is used to cycle the subsystem targeting.
            if (TmThrottleController.TestButtonDown(e.Buttons, button07) &&
               (TmThrottleController.SharedState.CurrentMode == EliteSharedState.Mode.Travel ||
                TmThrottleController.SharedState.CurrentMode == EliteSharedState.Mode.Mining))
            {
                TmThrottleController.SharedState.SecondaryFireActive = true;
                TmThrottleController.SetJoystickButton(true, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
            else if (TmThrottleController.SharedState.SecondaryFireActive)
            {
                TmThrottleController.SharedState.SecondaryFireActive = false;
                TmThrottleController.SetJoystickButton(false, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
        }
    }
}
