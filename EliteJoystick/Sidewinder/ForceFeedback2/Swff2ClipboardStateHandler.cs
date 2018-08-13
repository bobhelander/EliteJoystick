using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystick.Sidewinder.ForceFeedback2
{
    public class Swff2ClipboardStateHandler : StateHandler
    {
        static readonly UInt32 button6 = (UInt32)Faz.SideWinderSC.Logic.Swff2Button.Button6;

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
            if (Swff2Controller.TestButtonPressed(e.PreviousButtons, e.Buttons, button6))
            {
                if (System.Windows.Clipboard.ContainsText())
                {
                    swff2Controller.TypeFromClipboard();
                }
            }
        }
    }
}
