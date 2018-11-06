using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleSliderJoystick : IObserver<States>
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
            // 0-127 0 = up  127 = down

            int slider = current.Slider * 32;           
            Controller.SetJoystickAxis(slider, HID_USAGES.HID_USAGE_SL0, vJoyTypes.Throttle);
        }
    }
}
