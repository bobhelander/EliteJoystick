using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleClearMessages : IObserver<States>
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

        static uint pinkyBack = (UInt32)Button.Button14;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (Controller.TestButtonPressed(previous.buttons, current.buttons, pinkyBack))
            {
                Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.TextMessageEntry, 200);
                Controller.TypeFullString("/clear");
                log.Debug("Clear Message Log");
            }
        }
    }
}
