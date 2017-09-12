﻿using EliteJoystick.Thrustmaster.Warthog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystick
{
    public class TmThrottleLightsCommand : StateHandler
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
            // Fuel Left : Lights
            var button = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button16;

            if (TmThrottleController.TestButtonChanged(e.PreviousButtons, e.Buttons, button))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LightsToggle, 200);
                TmThrottleController.VisualState.UpdateMessage("Lights On");
            }
            if (TmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, button))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LightsToggle, 200);
                tmThrottleController.VisualState.UpdateMessage("Lights Off");
            }
        }
    }
}
