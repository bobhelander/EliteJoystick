using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2ZJoystick
    {
        public static void Process(States value, Controller controller)
        {
            // twist = -32 to 31

            var current = value.Current as State;

            int z = ((current.R * -1) + 32) * 16 * 32;

            z = Curves.Calculate(z - (16 * 1024), (16 * 1024), .4) + 16 * 1024;

            controller.SetJoystickAxis(z, HID_USAGES.HID_USAGE_Z, vJoyTypes.StickAndPedals);
        }
    }
}
