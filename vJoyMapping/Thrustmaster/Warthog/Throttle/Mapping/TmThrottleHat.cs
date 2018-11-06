using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleHat : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public List<uint> vButtons { get; set; } = new List<uint> {
            MappedButtons.ThrottleHatUp,
            MappedButtons.ThrottleHatRight,
            MappedButtons.ThrottleHatDown,
            MappedButtons.ThrottleHatLeft };

        public void OnNext(States value)
        {
            var current = value.Current as State;

            int index = 0;
            foreach (int hatValue in Enum.GetValues(typeof(Hat)))
            {
                bool pressed = (current.Hat & (int)hatValue) != 0;
                Controller.SetJoystickButton(pressed, vButtons[index], vJoyTypes.Virtual);
                index++;
            }

            int pov = current.Hat / 2;
            pov = pov == 4 ? -1 : pov;
            Controller.SetJoystickHat(pov, vJoyTypes.Throttle, 1);
        }
    }
}
