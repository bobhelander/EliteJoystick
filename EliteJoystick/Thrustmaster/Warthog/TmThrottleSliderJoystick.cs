using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Thrustmaster.Warthog
{
    public class TmThrottleSliderJoystick : Axis 
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
                    tmThrottleController.Controller.Slider += Controller_Slider;
                }
            }
        }

        // 0-127 0 = up  127 = down

        private void Controller_Slider(object sender, SliderEventArgs e)
        {
            int slider = e.Slider * 32;
            this.tmThrottleController.VisualState.UpdateAxis3((UInt32)slider);
            tmThrottleController.SetJoystickAxis(slider, HID_USAGES.HID_USAGE_SL0, vJoyTypes.Throttle);
        }
    }
}
