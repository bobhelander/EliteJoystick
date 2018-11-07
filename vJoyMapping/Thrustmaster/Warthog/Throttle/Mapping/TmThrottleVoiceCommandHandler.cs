﻿using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleVoiceCommandHandler
    {
        static UInt32 MIC = (UInt32)Button.Button02;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (controller.TestButtonPressed(previous.buttons, current.buttons, MIC))
            {
                controller.SharedState.Mute = true;
                controller.DepressKey(0xC3);  // KEY_F2    
            }
            if (controller.TestButtonReleased(previous.buttons, current.buttons, MIC))
            {
                controller.SharedState.Mute = false;
                controller.ReleaseKey(0xC3);  // KEY_F2
            }
        }
    }
}
