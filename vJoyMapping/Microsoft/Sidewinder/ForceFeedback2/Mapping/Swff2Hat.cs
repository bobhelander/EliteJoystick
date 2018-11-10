using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2Hat
    {
        private static List<uint> vButtons { get; set; } = new List<uint> {
            MappedButtons.ForceFeedback2HatUp,
            MappedButtons.ForceFeedback2HatRight,
            MappedButtons.ForceFeedback2HatDown,
            MappedButtons.ForceFeedback2HatLeft,
            MappedButtons.ForceFeedback2HatCentered};

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (current.Hat == previous.Hat)
                return; // No Change

            int pov = current.Hat / 2;
            pov = pov == 4 ? -1 : pov;

            controller.VirtualJoysticks.SetJoystickHat(pov, 1, 1);

            foreach (var vButton in vButtons)
            {
                controller.SetJoystickButton(false, vButton, vJoyTypes.StickAndPedals);
            }

            switch (current.Hat)
            {
                case 0: // up
                    controller.SetJoystickButton(true, vButtons[0], vJoyTypes.StickAndPedals);
                    break;
                case 1: // up - right
                    controller.SetJoystickButton(true, vButtons[0], vJoyTypes.StickAndPedals);
                    controller.SetJoystickButton(true, vButtons[1], vJoyTypes.StickAndPedals);
                    break;
                case 2: // right
                    controller.SetJoystickButton(true, vButtons[1], vJoyTypes.StickAndPedals);
                    break;
                case 3: // right down
                    controller.SetJoystickButton(true, vButtons[1], vJoyTypes.StickAndPedals);
                    controller.SetJoystickButton(true, vButtons[2], vJoyTypes.StickAndPedals);
                    break;
                case 4: // down
                    controller.SetJoystickButton(true, vButtons[2], vJoyTypes.StickAndPedals);
                    break;
                case 5: // left down
                    controller.SetJoystickButton(true, vButtons[2], vJoyTypes.StickAndPedals);
                    controller.SetJoystickButton(true, vButtons[3], vJoyTypes.StickAndPedals);
                    break;
                case 6: // left
                    controller.SetJoystickButton(true, vButtons[3], vJoyTypes.StickAndPedals);
                    break;
                case 7: // left up
                    controller.SetJoystickButton(true, vButtons[3], vJoyTypes.StickAndPedals);
                    controller.SetJoystickButton(true, vButtons[0], vJoyTypes.StickAndPedals);
                    break;
                case 8: // none
                    controller.SetJoystickButton(true, vButtons[4], vJoyTypes.StickAndPedals);
                    break;
            }   
        }
    }
}
