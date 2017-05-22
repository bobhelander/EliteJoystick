using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Thrustmaster.Warthog
{
    public class TmThrottleXYJoystick : Axis
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
                    tmThrottleController.Controller.Move += Controller_Move;
                }
            }
        }

        // x = -512 to 511
        // y = -512 to 511
        // z = -512 to 511

        private void Controller_Move(object sender, MoveEventArgs e)
        {
            int y = e.Y * 32;
            int x = e.X * 32;

            tmThrottleController.SetJoystickAxis(x, HID_USAGES.HID_USAGE_X, vJoyTypes.Throttle);
            tmThrottleController.SetJoystickAxis(y, HID_USAGES.HID_USAGE_Y, vJoyTypes.Throttle);
        }
    }
}
