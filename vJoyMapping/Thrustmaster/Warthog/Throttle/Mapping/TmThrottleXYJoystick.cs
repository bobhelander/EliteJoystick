using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleXYJoystick : IObserver<States>
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

            // x = -512 to 511
            // y = -512 to 511
            // z = -512 to 511

            int y = current.Y * 32;
            int x = current.X * 32;

            Controller.SetJoystickAxis(x, HID_USAGES.HID_USAGE_X, vJoyTypes.Throttle);
            Controller.SetJoystickAxis(y, HID_USAGES.HID_USAGE_Y, vJoyTypes.Throttle);
        }
    }
}
