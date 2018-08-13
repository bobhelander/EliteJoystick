using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.ForceFeedback2
{
    public class Swff2UtilCommandsStateHandler : StateHandler
    {
        static readonly UInt32 Button8 = (UInt32)Faz.SideWinderSC.Logic.Swff2Button.Button8;
        static readonly UInt32 Button7 = (UInt32)Faz.SideWinderSC.Logic.Swff2Button.Button7;
        static readonly UInt32 Button6 = (UInt32)Faz.SideWinderSC.Logic.Swff2Button.Button6;
        static readonly UInt32 Button5 = (UInt32)Faz.SideWinderSC.Logic.Swff2Button.Button5;
        static readonly UInt32 Button2 = (UInt32)Faz.SideWinderSC.Logic.Swff2Button.Button2;

        private Swff2Controller swff2Controller;

        public Swff2Controller Swff2Controller
        {
            get { return swff2Controller; }
            set
            {
                swff2Controller = value;
                if (null != swff2Controller)
                {
                    swff2Controller.Controller.ButtonsChanged += async (s, e) =>
                        await Task.Run(() => ControllerButtonsChanged(s, e));
                }
            }
        }

        private void ControllerButtonsChanged(object sender, Faz.SideWinderSC.Logic.ButtonsEventArgs e)
        {
            if (Swff2Controller.TestButtonPressed(e.PreviousButtons, e.Buttons, Button8))
            {
                // Oculus ASW off  CRTL+KP1
                Swff2Controller.SendKeyCombo(new byte[] { 0x80 }, 0x31);
            }
            if (Swff2Controller.TestButtonPressed(e.PreviousButtons, e.Buttons, Button7))
            {
                // Take Picture  ALT-F10
                Swff2Controller.SendKeyCombo(new byte[] { 0x82 }, 0xCB);
            }

            if (Swff2Controller.TestButtonPressed(e.PreviousButtons, e.Buttons, Button5))
            {
                // Focus Elite Window
                ClientActions.FocusProcess(this);
            }
                        
            if (Swff2Controller.TestButtonPressed(e.PreviousButtons, e.Buttons, Button2) &&
                Swff2Controller.SharedState.ThrottleShiftStateValue == EliteSharedState.ThrottleShiftState.Shift1)
            {
                // Focus Elite Window
                ClientActions.FocusProcess(this);
            }
        }
    }
}
