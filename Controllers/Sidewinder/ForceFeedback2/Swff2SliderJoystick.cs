using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.ForceFeedback2
{
    public class Swff2SliderJoystick : Axis 
    {
        private Swff2Controller swff2Controller;

        bool ProcessSet { get; set; }

        public Swff2Controller Swff2Controller
        {
            get { return swff2Controller; }
            set
            {
                swff2Controller = value;
                if (null != swff2Controller)
                {
                    swff2Controller.Controller.Slider += Controller_Slider;
                }
            }
        }

        // 0-127 0 = up  127 = down

        private void Controller_Slider(object sender, SliderEventArgs e)
        {
            int slider = e.Slider * 8 * 32;
            swff2Controller.SetJoystickAxis(slider, HID_USAGES.HID_USAGE_SL0, vJoyTypes.StickAndPedals);

            // Switch to Elite
            if (false == ProcessSet && slider > 30000)
            {
                ProcessSet = true;
                Utils.FocusWindow("EliteDangerous64");
            }
            else if (ProcessSet && slider < 30000)
            {
                ProcessSet = false;
            }
        }
    }
}
