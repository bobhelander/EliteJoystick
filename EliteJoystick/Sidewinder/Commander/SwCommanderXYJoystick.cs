using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Sidewinder.Commander
{
    public class SwCommanderXYJoystick : Axis
    {
        private ScController scController;

        public ScController ScController
        {
            get { return scController; }
            set
            {
                scController = value;
                if (null != scController)
                {
                    scController.Controller.Move += Controller_Move;
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

            this.scController.VisualState.UpdateAxis1((UInt32)x);
            this.scController.VisualState.UpdateAxis2((UInt32)y);

            x = Curves.Calculate(x - (16 * 1024), (16 * 1024), .5) + 16 * 1024;
            y = Curves.Calculate(y - (16 * 1024), (16 * 1024), .5) + 16 * 1024;

            scController.SetJoystickAxis(x, HID_USAGES.HID_USAGE_X, vJoyTypes.Commander);
            scController.SetJoystickAxis(y, HID_USAGES.HID_USAGE_Y, vJoyTypes.Commander);
        }
    }
}
