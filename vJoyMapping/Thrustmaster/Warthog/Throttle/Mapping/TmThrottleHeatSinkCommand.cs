using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleHeatSinkCommand
    {
        static readonly UInt32 BoatBack = (UInt32)Button.Button10;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, BoatBack))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.HeatSink, 200);
            }
        }
    }
}
