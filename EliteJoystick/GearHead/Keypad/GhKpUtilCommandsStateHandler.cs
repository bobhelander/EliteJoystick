using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.GearHead.Keypad
{
    public class GhKpUtilCommandsStateHandler : StateHandler
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
            if ((e.Buttons & (uint)Faz.SideWinderSC.Logic.KeypadButton.ButtonHtml) ==
                (uint)Faz.SideWinderSC.Logic.KeypadButton.ButtonHtml)
            {
                // Oculus ASW off  CRTL+KP1
                ghKpController.SendKeyCombo(new byte[] { 0x80 }, 0x31);
            }
        }
    }
}
