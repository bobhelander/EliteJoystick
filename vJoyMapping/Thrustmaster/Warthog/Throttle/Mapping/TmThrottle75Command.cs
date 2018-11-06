using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottle75Command : IObserver<States>
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        static UInt32 rdrAlt = (UInt32)Button.Button25;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (Controller.TestButtonReleased(previous.buttons, current.buttons, rdrAlt))
            {
                Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.Throttle75, 200);
                log.Debug("75% throttle");
            }
        }
    }
}
