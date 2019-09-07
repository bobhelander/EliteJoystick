using EliteJoystick.Common;
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
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static UInt32 rdrAlt = (UInt32)Button.Button25;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonReleased(value, rdrAlt))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.Throttle75, 200);
                log.Debug("75% throttle");
            }
        }
    }
}
