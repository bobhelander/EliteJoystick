using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Mapping
{
    public class SwCommanderButtonStateHandler : IObserver<States>
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
            foreach (Button button in Enum.GetValues(typeof(Button)))
            {
                bool pressed = ((current.Buttons & (int)button) == (int)button);
                Controller.SetJoystickButton(pressed, buttonIndex, vJoyTypes.StickAndPedals);
                buttonIndex++;
            }
        }
    }
}
