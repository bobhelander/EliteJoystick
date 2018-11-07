using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2SliderJoystick
    {
        private static bool processSet = false;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;

            // 0-127 0 = up  127 = down

            int slider = current.Slider * 8 * 32;
            controller.SetJoystickAxis(slider, HID_USAGES.HID_USAGE_SL0, vJoyTypes.StickAndPedals);

            // Switch to Elite
            if (false == processSet && slider > 30000)
            {
                processSet = true;
                Utils.FocusWindow("EliteDangerous64");
            }
            else if (processSet && slider < 30000)
            {
                processSet = false;
            }
        }
    }
}
