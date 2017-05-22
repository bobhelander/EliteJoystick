using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Thrustmaster.Warthog
{
    public class TmThrottleMuteHandler : StateHandler
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
            UInt32 mute = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button02;

            if ((e.Buttons & mute) == mute && (e.PreviousButtons & mute) == 0)
            {
                tmThrottleController.SharedState.Mute = true;
            }
            if ((e.Buttons & mute) == 0 && (e.PreviousButtons & mute) == mute)
            {
                tmThrottleController.SharedState.Mute = false;
            }
        }
    }
}
