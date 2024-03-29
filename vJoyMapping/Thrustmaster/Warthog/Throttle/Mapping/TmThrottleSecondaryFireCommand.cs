﻿using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleSecondaryFireCommand
    {
        private const UInt32 SpeedbrakeForward = (UInt32)Button.Button07;

        public static void Process(States value, Controller controller)
        {
            // Use the speedbrake to lock the secondary fire button down.  
            // This will hold the mining laser or discovery scanner on until the switch is moved off.
            // In fighting mode this switch is used to cycle the subsystem targeting.

            if (Reactive.ButtonDown(value, SpeedbrakeForward) &&
               (controller.SharedState.CurrentMode == EliteSharedState.Mode.Travel ||
                controller.SharedState.CurrentMode == EliteSharedState.Mode.Mining))
            {
                controller.SharedState.SecondaryFireActive = true;
                controller.DepressKey(0x82);  // Left Alt
                controller.DepressKey(0x84);  // Right Ctrl
                controller.DepressKey(0x4C);  // L
                controller.SetJoystickButton(true, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
            else if (controller.SharedState.SecondaryFireActive)
            {
                controller.SharedState.SecondaryFireActive = false;
                controller.ReleaseKey(0x4C);  // L
                controller.ReleaseKey(0x84);  // Right Ctrl
                controller.ReleaseKey(0x82);  // Left Alt
                controller.SetJoystickButton(false, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
        }
    }
}
