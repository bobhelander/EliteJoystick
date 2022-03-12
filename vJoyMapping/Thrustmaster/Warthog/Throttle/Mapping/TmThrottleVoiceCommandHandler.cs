using EliteJoystick.Common;
using EliteJoystick.Common.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleVoiceCommandHandler
    {
        private const UInt32 MIC = (UInt32)Button.Button02;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, MIC))
            {
                controller.SharedState.Mute = true;
                // KEY_F9
                controller.PressKey(KeyMap.KeyNameMap["KEY_F9"].Code, null, -1);
            }
            if (Reactive.ButtonReleased(value, MIC))
            {
                controller.SharedState.Mute = false;
                controller.ReleaseKey(KeyMap.KeyNameMap["KEY_F9"].Code, null);
            }
        }
    }
}
