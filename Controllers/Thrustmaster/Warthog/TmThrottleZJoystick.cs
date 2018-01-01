using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleZJoystick : Axis 
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
                    tmThrottleController.Controller.ThrottleRight += Controller_ThrottleRight;
                    tmThrottleController.Controller.ThrottleLeft += Controller_ThrottleLeft;
                }
            }
        }

        public bool LeftEnabled { get; set; }

        public bool RightEnabled { get; set; }

        private void Controller_ThrottleLeft(object sender, ThrottleAxisEventArgs e)
        {
            if (LeftEnabled)
            {
                int z = e.AxisValue * 2;

                //this.tmThrottleController.VisualState.UpdateAxis1((UInt32)z);

                // for this curve we want the top and bottom of the throttle to be the full multiplier
                // The middle is where it will damper the movement

                // Shift the value down to -16k to +16K
                int shifted_value = z - (1024 * 16);

                // .2 makes a good dead zone in the middle of the throttle
                shifted_value = Curves.Calculate(shifted_value, (16 * 1024), .2);

                // Move the value back up
                z = shifted_value + (1024 * 16);

                tmThrottleController.SetJoystickAxis(z, HID_USAGES.HID_USAGE_RZ, vJoyTypes.Throttle);
            }
        }

        private void Controller_ThrottleRight(object sender, ThrottleAxisEventArgs e)
        {
            if (RightEnabled)
            {
                int z = e.AxisValue * 2;

                //this.tmThrottleController.VisualState.UpdateAxis2((UInt32)z);

                // Zero is full throttle
                // 32K is no throttle

                // Invert the values.  We want the multipler to rise the closer we get to full throttle
                //int z_inverted = (1024 * 32) - z;

                //z_inverted = Curves.Calculate(z_inverted, (32 * 1024), .5);

                // Invert the value again
                //z = (1024 * 32) - z_inverted;

                tmThrottleController.SetJoystickAxis(z, HID_USAGES.HID_USAGE_Z, vJoyTypes.Throttle);
            }
        }
    }
}
