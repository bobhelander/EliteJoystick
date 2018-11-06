using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleSilentCommand : IObserver<States>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        // Fuel Right : Silent
        static readonly UInt32 FuelRight = (UInt32)Button.Button17;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (Controller.TestButtonPressed(previous.buttons, current.buttons, FuelRight))
            {
                Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                log.Debug($"Silent Running Activated");
            }
            if (Controller.TestButtonReleased(previous.buttons, current.buttons, FuelRight))
            {
                Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                log.Debug($"Silent Running Deactivated");
            }
        }
    }
}
