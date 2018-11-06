using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public class Swff2XYJoystick : IObserver<States>
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

            int y = ((current.Y * -1) + 511) * 32;
            int x = ((current.X * -1) + 511) * 32;

            x = Curves.Calculate(x - (16 * 1024), (16 * 1024), .4) + 16 * 1024;
            y = Curves.Calculate(y - (16 * 1024), (16 * 1024), .4) + 16 * 1024;

            Controller.SetJoystickAxis(x, HID_USAGES.HID_USAGE_X, vJoyTypes.StickAndPedals);
            Controller.SetJoystickAxis(y, HID_USAGES.HID_USAGE_Y, vJoyTypes.StickAndPedals);
        }
    }
}
