using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.GearHead.Keypad
{
    public class GhKpButtonsStateHandler : StateHandler
    {
        private GhKpController ghKpController;

        public GhKpController GhKpController
        {
            get { return ghKpController; }
            set
            {
                ghKpController = value;
                if (null != ghKpController)
                {
                    ghKpController.Controller.ButtonsChanged += Controller_ButtonsChanged;
                }
            }
        }

        private void Controller_ButtonsChanged(object sender, Faz.SideWinderSC.Logic.ButtonsEventArgs e)
        {
            uint buttonIndex = 19;
            foreach (Faz.SideWinderSC.Logic.KeypadButton value in Enum.GetValues(typeof(Faz.SideWinderSC.Logic.KeypadButton)))
            {
                bool pressed = ((e.Buttons & (int)value) == (int)value);
                ghKpController.SetJoystickButton(pressed, buttonIndex, vJoyTypes.Virtual);
                buttonIndex++;
            }

            this.ghKpController.VisualState.UpdateButtons(e.Buttons);
        }
    }
}
