using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottle75Command : StateHandler
    {
        static UInt32 rdrAlt = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button25;

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
            if (TmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, rdrAlt))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.Throttle75, 200);
            }
        }
    }
}
