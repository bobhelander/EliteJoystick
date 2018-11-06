using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public class Swff2Hat : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public List<uint> vButtons { get; set; }

        public void OnNext(States value)
        {
            var current = value.Current as State;

            int pov = current.Hat / 2;
            pov = pov == 4 ? -1 : pov;
            Controller.vJoy.SetDiscPov(pov, 1, 1);

            foreach (var vButton in vButtons)
            {
                Controller.SetJoystickButton(false, vButton, vJoyTypes.StickAndPedals);
            }

            switch (current.Hat)
            {
                case 0: // up
                    Controller.SetJoystickButton(true, vButtons[0], vJoyTypes.StickAndPedals);
                    break;
                case 1: // up - right
                    Controller.SetJoystickButton(true, vButtons[0], vJoyTypes.StickAndPedals);
                    Controller.SetJoystickButton(true, vButtons[1], vJoyTypes.StickAndPedals);
                    break;
                case 2: // right
                    Controller.SetJoystickButton(true, vButtons[1], vJoyTypes.StickAndPedals);
                    break;
                case 3: // right down
                    Controller.SetJoystickButton(true, vButtons[1], vJoyTypes.StickAndPedals);
                    Controller.SetJoystickButton(true, vButtons[2], vJoyTypes.StickAndPedals);
                    break;
                case 4: // down
                    Controller.SetJoystickButton(true, vButtons[2], vJoyTypes.StickAndPedals);
                    break;
                case 5: // left down
                    Controller.SetJoystickButton(true, vButtons[2], vJoyTypes.StickAndPedals);
                    Controller.SetJoystickButton(true, vButtons[3], vJoyTypes.StickAndPedals);
                    break;
                case 6: // left
                    Controller.SetJoystickButton(true, vButtons[3], vJoyTypes.StickAndPedals);
                    break;
                case 7: // left up
                    Controller.SetJoystickButton(true, vButtons[3], vJoyTypes.StickAndPedals);
                    Controller.SetJoystickButton(true, vButtons[0], vJoyTypes.StickAndPedals);
                    break;
                case 8: // none
                    Controller.SetJoystickButton(true, vButtons[4], vJoyTypes.StickAndPedals);
                    break;
            }   
        }
    }
}
