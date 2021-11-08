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

            // Z is off by about -13 to -15
            var zRaw = current.R;
            if (zRaw > 108 && zRaw < 128)
                zRaw = 128;

            int y = current.Y * 128;
            int x = current.X * 128;
            int z = zRaw * 128;

            // Add X an Y together to use the toe brakes to go up and down

            int combined = ((128 * 255) / 2) + (current.Y * 64 - current.X * 64);
            combined = Curves.Calculate(combined - (16 * 1024), (16 * 1024), .2) + 16 * 1024;

            controller.SetJoystickAxis(combined, (int)vJoy.Wrapper.Axis.HID_USAGE_RX, vJoyTypes.StickAndPedals);
            controller.SetJoystickAxis(z, (int)vJoy.Wrapper.Axis.HID_USAGE_RZ, vJoyTypes.StickAndPedals);
        }

        public static void Process(AltStates value, Controller controller)
        {
            var current = value.Current as State;

            // x = -512 to 511
            // y = -512 to 511
            // z = -512 to 511
            // z = 0 to 255

            // Z is off by about -13 to -15
            var zRaw = current.R;
            if (zRaw > 108 && zRaw < 128)
                zRaw = 128;

            int y = current.Y * 128;
            int x = current.X * 128;
            int z = zRaw * 128;

            // Add X an Y together to use the toe brakes to go up and down

            int combined = ((128 * 255) / 2) + (current.Y * 64 - current.X * 64);
            combined = Curves.Calculate(combined - (16 * 1024), (16 * 1024), .2) + 16 * 1024;

            controller.SetJoystickAxis(combined, (int)vJoy.Wrapper.Axis.HID_USAGE_RX, vJoyTypes.StickAndPedals);
            controller.SetJoystickAxis(z, (int)vJoy.Wrapper.Axis.HID_USAGE_RZ, vJoyTypes.StickAndPedals);
        }
    }
}
