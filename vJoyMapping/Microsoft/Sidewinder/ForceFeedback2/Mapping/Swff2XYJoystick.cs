using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using EliteJoystick.Common;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2XYJoystick
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;

            int y = ((current.Y * -1) + 511) * 32;      // -511 - +512  (Shift value up to 0 - 1024) Then multiply by 32
            int x = ((current.X * -1) + 511) * 32;      // -511 - +512
            double minPercent = current.Slider / 127;   // 0 - 127  Control the curve with the slider

            x = Curves.Calculate(x - (16 * 1024), (16 * 1024), minPercent) + 16 * 1024;
            y = Curves.Calculate(y - (16 * 1024), (16 * 1024), minPercent) + 16 * 1024;

            controller.SetJoystickAxis(x, (int)vJoy.Wrapper.Axis.HID_USAGE_X, vJoyTypes.StickAndPedals);
            controller.SetJoystickAxis(y, (int)vJoy.Wrapper.Axis.HID_USAGE_Y, vJoyTypes.StickAndPedals);
        }
    }
}
