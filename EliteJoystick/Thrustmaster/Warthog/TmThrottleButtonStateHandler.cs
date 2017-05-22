using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Thrustmaster.Warthog
{
    public class TmThrottleButtonStateHandler : StateHandler
    {
        public uint vButtonStart { get; set; }

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
            uint buttonIndex = 1;
            foreach (Faz.SideWinderSC.Logic.TmThrottleButton value in Enum.GetValues(typeof(Faz.SideWinderSC.Logic.TmThrottleButton)))
            {
                bool buttonPressed = (e.Buttons & (uint)value) == (uint)value;
                TmThrottleController.SetJoystickButton(buttonPressed, buttonIndex, vJoyTypes.Throttle);
                //TmThrottleController.SetJoystickButton(!buttonPressed, buttonIndex, 1);
                buttonIndex++;
            }

            buttonIndex = 1;
            foreach (Faz.SideWinderSC.Logic.TmThrottleSwitchNeutral value in Enum.GetValues(typeof(Faz.SideWinderSC.Logic.TmThrottleSwitchNeutral)))
            {
                bool buttonPressed = (e.Buttons & (uint)value) != 0;
                TmThrottleController.SetJoystickButton(!buttonPressed, buttonIndex, vJoyTypes.Virtual);
                buttonIndex++;
            }

            this.tmThrottleController.VisualState.UpdateButtons(e.Buttons);
        }
    }
}
