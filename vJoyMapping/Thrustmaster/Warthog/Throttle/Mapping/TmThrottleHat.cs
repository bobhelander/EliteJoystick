using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleHat
    {
        public static List<uint> vButtons { get; set; } = new List<uint> {
            MappedButtons.ThrottleHatUp,
            MappedButtons.ThrottleHatRight,
            MappedButtons.ThrottleHatDown,
            MappedButtons.ThrottleHatLeft };

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (current.Hat == previous.Hat)
                return; // No Change

            int index = 0;
            foreach (int hatValue in Enum.GetValues(typeof(Hat)))
            {
                bool pressed = (current.Hat & (int)hatValue) != 0;
                controller.SetJoystickButton(pressed, vButtons[index], vJoyTypes.StickAndPedals);
                index++;
            }

            int pov = current.Hat / 2;
            pov = pov == 4 ? -1 : pov;
            controller.SetJoystickHat(pov, vJoyTypes.Throttle, 1);
        }
    }
}
