using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleSilentCommand
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Fuel Right : Silent
        static readonly UInt32 FuelRight = (UInt32)Button.Button17;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (controller.TestButtonPressed(previous.buttons, current.buttons, FuelRight))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                log.Debug($"Silent Running Activated");
            }
            if (controller.TestButtonReleased(previous.buttons, current.buttons, FuelRight))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                log.Debug($"Silent Running Deactivated");
            }
        }
    }
}
