using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.CHProducts.ProPedals.Models;
using vJoyMapping.Common;

namespace vJoyMapping.CHProducts.ProPedals.Mapping
{
    public static class ChPedalsXYR
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;

            // x = -512 to 511
            // y = -512 to 511
            // z = -512 to 511
            // z = 0 to 255

            int y = current.Y * 128;
            int x = current.X * 128;
            int z = current.R * 128;

            // Add X an Y together to use the toe brakes to go up and down

            int combined = ((128 * 255) / 2) + (current.Y * 64 - current.X * 64);
            combined = Curves.Calculate(combined - (16 * 1024), (16 * 1024), .2) + 16 * 1024;

            controller.SetJoystickAxis(combined, HID_USAGES.HID_USAGE_RX, vJoyTypes.StickAndPedals);
            controller.SetJoystickAxis(z, HID_USAGES.HID_USAGE_RZ, vJoyTypes.StickAndPedals);
        }
    }
}
