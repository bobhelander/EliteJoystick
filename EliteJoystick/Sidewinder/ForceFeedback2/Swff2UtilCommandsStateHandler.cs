using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Sidewinder.ForceFeedback2
{
    public class Swff2UtilCommandsStateHandler : StateHandler
    {
        private Swff2Controller swff2Controller;

        public Swff2Controller Swff2Controller
        {
            get { return swff2Controller; }
            set
            {
                swff2Controller = value;
                if (null != swff2Controller)
                {
                    swff2Controller.Controller.ButtonsChanged += Controller_ButtonsChanged;
                }
            }
        }

        private void Controller_ButtonsChanged(object sender, Faz.SideWinderSC.Logic.ButtonsEventArgs e)
        {
            if ((e.Buttons & (uint)Faz.SideWinderSC.Logic.Swff2Button.Button8) == 
                (uint)Faz.SideWinderSC.Logic.Swff2Button.Button8)
            {
                // Oculus ASW off  CRTL+KP1
                swff2Controller.SendKeyCombo(new byte[] { 0x80 }, 0x31);
            }
            if ((e.Buttons & (uint)Faz.SideWinderSC.Logic.Swff2Button.Button7) == 
                (uint)Faz.SideWinderSC.Logic.Swff2Button.Button7)
            {
                // Take Picture  ALT-F10
                swff2Controller.SendKeyCombo(new byte[] { 0x82 }, 0xCB);
            }

            if ((e.Buttons & (uint)Faz.SideWinderSC.Logic.Swff2Button.Button5) ==
                (uint)Faz.SideWinderSC.Logic.Swff2Button.Button5)
            {
                // HUD off  CRTL+ALT+G
                swff2Controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47);
            }

            if (((e.Buttons & (uint)Faz.SideWinderSC.Logic.Swff2Button.Button2) ==
                  (uint)Faz.SideWinderSC.Logic.Swff2Button.Button2) &&
                 (e.PreviousButtons & (uint)Faz.SideWinderSC.Logic.Swff2Button.Button2) == 0 &&
                 swff2Controller.SharedState.ThrottleShiftStateValue == EliteSharedState.ThrottleShiftState.Shift1)
            {
                // Focus window
                System.Console.WriteLine(Utils.FocusWindow("EliteDangerous64"));
            }
        }
    }
}
