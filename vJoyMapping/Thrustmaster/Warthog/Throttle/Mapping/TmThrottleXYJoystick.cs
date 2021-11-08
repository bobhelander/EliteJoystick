using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleXYJoystick
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;

            // x = -512 to 511
            // y = -512 to 511
            // z = -512 to 511

            int y = current.Y * 32;
            int x = current.X * 32;

            controller.SetJoystickAxis(x, (int)vJoy.Wrapper.Axis.HID_USAGE_X, vJoyTypes.Throttle);
            controller.SetJoystickAxis(y, (int)vJoy.Wrapper.Axis.HID_USAGE_Y, vJoyTypes.Throttle);
        }
    }
}
