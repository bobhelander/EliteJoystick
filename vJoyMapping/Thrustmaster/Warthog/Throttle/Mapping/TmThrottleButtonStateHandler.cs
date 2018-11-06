using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleButtonStateHandler : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(States value)
        {
            var current = value.Current as State;

            uint buttonIndex = 1;
            foreach (uint button in Enum.GetValues(typeof(Button)))
            {
                bool buttonPressed = (current.buttons & (uint)button) == (uint)button;
                Controller.SetJoystickButton(buttonPressed, buttonIndex, vJoyTypes.Throttle);
                buttonIndex++;
            }

            buttonIndex = 1;
            foreach (UInt32 nbutton in Enum.GetValues(typeof(SwitchNeutral)))
            {
                bool buttonPressed = (current.buttons & (uint)nbutton) != 0;
                Controller.SetJoystickButton(!buttonPressed, buttonIndex, vJoyTypes.Virtual);
                buttonIndex++;
            }
        }
    }
}
