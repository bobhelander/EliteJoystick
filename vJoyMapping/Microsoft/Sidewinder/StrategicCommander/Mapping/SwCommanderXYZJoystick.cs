using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Models;
using EliteJoystick.Common;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Mapping
{
    public static class SwCommanderXYZJoystick
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;

            // x = -512 to 511
            // y = -512 to 511
            // z = -512 to 511

            int y = ((current.Y * -1) + 511) * 32;
            int x = ((current.X * -1) + 511) * 32;
            int z = ((current.R * -1) + 511) * 32;

            x = Curves.Calculate(x - (16 * 1024), (16 * 1024), .5) + 16 * 1024;
            y = Curves.Calculate(y - (16 * 1024), (16 * 1024), .5) + 16 * 1024;

            controller.SetJoystickAxis(x, HID_USAGES.HID_USAGE_X, vJoyTypes.Commander);
            controller.SetJoystickAxis(y, HID_USAGES.HID_USAGE_Y, vJoyTypes.Commander);
            controller.SetJoystickAxis(z, HID_USAGES.HID_USAGE_Z, vJoyTypes.Commander);
        }
    }
}
