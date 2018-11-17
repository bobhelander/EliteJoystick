using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleButtonStateHandler
    {
        public static void Process(States value, Controller controller)
        {
            uint buttonIndex = 1;
            foreach (uint button in Enum.GetValues(typeof(Button)))
            {
                controller.SetJoystickButton(
                    Reactive.ButtonDown(value, (uint)button), buttonIndex, vJoyTypes.Throttle);
                buttonIndex++;
            }

            buttonIndex = MappedButtons.ThrottleMSNone;
            foreach (UInt32 nbutton in Enum.GetValues(typeof(SwitchNeutral)))
            {
                bool buttonPressed = (value.Current.Buttons & (uint)nbutton) != 0;
                controller.SetJoystickButton(!buttonPressed, buttonIndex, vJoyTypes.StickAndPedals);
                buttonIndex++;
            }
        }
    }
}
