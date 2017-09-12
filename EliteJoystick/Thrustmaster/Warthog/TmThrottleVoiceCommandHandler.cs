using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Thrustmaster.Warthog
{
    public class TmThrottleVoiceCommandHandler : StateHandler
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
            UInt32 voiceCommandButton = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button02;

            if ((e.Buttons & voiceCommandButton) == voiceCommandButton && (e.PreviousButtons & voiceCommandButton) == 0)
            {
                tmThrottleController.SharedState.Mute = true;
                TmThrottleController.DepressKey(0xC3);  // KEY_F2    
            }
            if ((e.Buttons & voiceCommandButton) == 0 && (e.PreviousButtons & voiceCommandButton) == voiceCommandButton)
            {
                tmThrottleController.SharedState.Mute = false;
                TmThrottleController.ReleaseKey(0xC3);  // KEY_F2
            }
        }
    }
}
