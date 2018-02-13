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
                    tmThrottleController.Controller.SwitchState += async (s, e) =>
                        await Task.Run(() => ControllerSwitchState(s, e));
                }
            }
        }

        private void ControllerSwitchState(object sender, Faz.SideWinderSC.Logic.TmThrottleSwitchEventArgs e)
        {
            if (TmThrottleController.TestButtonPressedOrReleased(e.PreviousButtons, e.Buttons, button16))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LightsToggle, 200);
            }
        }
    }
}
