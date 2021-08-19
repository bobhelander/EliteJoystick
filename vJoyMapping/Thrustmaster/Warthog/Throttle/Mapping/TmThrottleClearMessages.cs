using EliteJoystick.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleClearMessages
    {
        private const uint pinkyBack = (UInt32)Button.Button14;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, pinkyBack))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.TextMessageEntry, 200);
                Task.Run(async () => await controller.TypeFullString("/clear").ConfigureAwait(false));
                controller.Logger.LogDebug("Clear Message Log");
            }
        }
    }
}
