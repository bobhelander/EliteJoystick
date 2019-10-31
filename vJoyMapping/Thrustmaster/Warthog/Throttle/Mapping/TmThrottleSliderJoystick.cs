using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleSliderJoystick
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (current.Slider == previous.Slider)
                return; // No Change

            // 0-127 0 = up  127 = down

            int slider = current.Slider * 32;
            controller.SetJoystickAxis(slider, vJoy.Wrapper.Axis.HID_USAGE_SL0, vJoyTypes.Throttle);
        }
    }
}
