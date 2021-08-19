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
    public static class TmThrottle75Command
    {
        static UInt32 rdrAlt = (UInt32)Button.Button25;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonReleased(value, rdrAlt))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.Throttle75, 200);
                controller.Logger.LogDebug("75% throttle");
            }
        }
    }
}
