using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public class Swff2SliderJoystick : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        bool ProcessSet { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        // 0-127 0 = up  127 = down

        public void OnNext(States value)
        {
            var current = value.Current as State;

            int slider = current.Slider * 8 * 32;
            Controller.SetJoystickAxis(slider, HID_USAGES.HID_USAGE_SL0, vJoyTypes.StickAndPedals);

            // Switch to Elite
            if (false == ProcessSet && slider > 30000)
            {
                ProcessSet = true;
                Utils.FocusWindow("EliteDangerous64");
            }
            else if (ProcessSet && slider < 30000)
            {
                ProcessSet = false;
            }
        }
    }
}
