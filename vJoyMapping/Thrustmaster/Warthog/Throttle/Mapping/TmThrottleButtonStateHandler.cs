using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleButtonStateHandler
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;
            
            if (current.buttons == previous.buttons)
                return; // No Change

            uint buttonIndex = 1;
            foreach (uint button in Enum.GetValues(typeof(Button)))
            {
                bool buttonPressed = (current.buttons & (uint)button) == (uint)button;
                controller.SetJoystickButton(buttonPressed, buttonIndex, vJoyTypes.Throttle);
                buttonIndex++;
            }

            buttonIndex = 1;
            foreach (UInt32 nbutton in Enum.GetValues(typeof(SwitchNeutral)))
            {
                bool buttonPressed = (current.buttons & (uint)nbutton) != 0;
                controller.SetJoystickButton(!buttonPressed, buttonIndex, vJoyTypes.Virtual);
                buttonIndex++;
            }
        }
    }
}
