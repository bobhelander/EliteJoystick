using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Sidewinder.ForceFeedback2
{
    public class Swff2XYJoystick : Axis
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
                    swff2Controller.Controller.Move += Controller_Move;
                }
            }
        }

        // x = -512 to 511
        // y = -512 to 511
        // z = -512 to 511

        private void Controller_Move(object sender, MoveEventArgs e)
        {
            int y = ((e.Y * -1) + 511) * 32;
            int x = ((e.X * -1) + 511) * 32;

            this.swff2Controller.VisualState.UpdateAxis1((UInt32)x);
            this.swff2Controller.VisualState.UpdateAxis2((UInt32)y);

            x = Curves.Calculate(x - (16 * 1024), (16 * 1024), .4) + 16 * 1024;
            y = Curves.Calculate(y - (16 * 1024), (16 * 1024), .4) + 16 * 1024;

            swff2Controller.SetJoystickAxis(x, HID_USAGES.HID_USAGE_X, vJoyTypes.StickAndPedals);
            swff2Controller.SetJoystickAxis(y, HID_USAGES.HID_USAGE_Y, vJoyTypes.StickAndPedals);
        }
    }
}
