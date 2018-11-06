using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public class Swff2ButtonsStateHandler : IObserver<States>
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
