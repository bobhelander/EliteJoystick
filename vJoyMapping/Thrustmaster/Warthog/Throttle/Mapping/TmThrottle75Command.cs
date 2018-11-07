using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottle75Command 
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static UInt32 rdrAlt = (UInt32)Button.Button25;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (controller.TestButtonReleased(previous.buttons, current.buttons, rdrAlt))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.Throttle75, 200);
                log.Debug("75% throttle");
            }
        }
    }
}
