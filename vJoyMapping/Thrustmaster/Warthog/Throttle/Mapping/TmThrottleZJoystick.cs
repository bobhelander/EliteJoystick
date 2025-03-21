﻿using EliteJoystick.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleZJoystick
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;

            if (controller.SharedState.LeftThrottleEnabled)
            {
                int z = current.Zr * 2;

                // for this curve we want the top and bottom of the throttle to be the full multiplier
                // The middle is where it will damper the movement

                // Shift the value down to -16k to +16K
                int shifted_value = z - (1024 * 16);

                // .2 makes a good dead zone in the middle of the throttle
                shifted_value = Curves.Calculate(shifted_value, (16 * 1024), .2);

                // Move the value back up
                z = shifted_value + (1024 * 16);

                controller.SetJoystickAxis(z, (int)vJoy.Wrapper.Axis.HID_USAGE_RZ, vJoyTypes.Throttle);
            }

            if (controller.SharedState.RightThrottleEnabled)
            {
                int z = current.Z * 2;

                // Zero is full throttle
                // 32K is no throttle

                // When the throttles split and Zr is is around 32K turn on reverse thrusting
                var difference = current.Zr - current.Z;
                if (current.Zr > 15500 && difference > 1000)
                {
                    if (controller.GetJoystickButton(MappedButtons.AlternateReverseToggle, vJoyTypes.Virtual) == false)
                    {
                        controller.Logger.LogDebug("Reverse Throttle On");
                        controller.SetJoystickButton(true, MappedButtons.AlternateReverseToggle, vJoyTypes.Virtual);
                    }
                }
                else
                {
                    if (controller.GetJoystickButton(MappedButtons.AlternateReverseToggle, vJoyTypes.Virtual))
                    {
                        controller.Logger.LogDebug("Reverse Throttle Off");
                        controller.SetJoystickButton(false, MappedButtons.AlternateReverseToggle, vJoyTypes.Virtual);
                    }
                }

                // Invert the values.  We want the multipler to rise the closer we get to full throttle
                //int z_inverted = (1024 * 32) - z;

                //z_inverted = Curves.Calculate(z_inverted, (32 * 1024), .5);

                // Invert the value again
                //z = (1024 * 32) - z_inverted;

                controller.SetJoystickAxis(z, (int)vJoy.Wrapper.Axis.HID_USAGE_Z, vJoyTypes.Throttle);
            }
        }
    }
}
