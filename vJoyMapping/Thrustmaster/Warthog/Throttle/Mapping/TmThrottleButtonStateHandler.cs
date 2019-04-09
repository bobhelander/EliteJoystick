﻿using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleButtonStateHandler
    {
        public static void Process(States value, Controller controller)
        {
            // Set normal buttons
            controller.SetJoystickButtons(value.Current.Buttons, vJoyTypes.Throttle, 0xFFFFFFFF);

            uint neutralButtons = 0;

            uint buttonIndex = MappedButtons.ThrottleMSNone;
            foreach (UInt32 nbutton in Enum.GetValues(typeof(SwitchNeutral)))
            {
                bool buttonPressed = (value.Current.Buttons & (uint)nbutton) == 0;

                // Set the position if down
                if (buttonPressed)
                    neutralButtons = neutralButtons | (uint)0x1 << (int)(buttonIndex - 1);
                buttonIndex++;
            }

            // Set the neutral position buttons
            controller.SetJoystickButtons(neutralButtons, vJoyTypes.StickAndPedals, MappedButtons.ThrottleNeutralMask);
        }
    }
}
