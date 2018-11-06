using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleLightsCommand : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        static readonly UInt32 FuelLeft = (UInt32)Button.Button16;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (Controller.TestButtonPressedOrReleased(previous.buttons, current.buttons, FuelLeft))
            {
                Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LightsToggle, 200);
            }
        }
    }
}
