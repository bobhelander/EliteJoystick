using EliteJoystick.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleSilentCommand
    {
        // Fuel Right : Silent
        static readonly UInt32 FuelRight = (UInt32)Button.Button17;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, FuelRight))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                controller.Logger.LogDebug($"Silent Running Activated");
            }
            if (Reactive.ButtonReleased(value, FuelRight))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                controller.Logger.LogDebug($"Silent Running Deactivated");
            }
        }
    }
}
