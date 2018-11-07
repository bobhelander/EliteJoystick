using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2ButtonsStateHandler
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (current.Buttons == previous.Buttons)
                return; // No Change

            uint buttonIndex = 1;
            foreach (Button button in Enum.GetValues(typeof(Button)))
            {
                bool pressed = ((current.Buttons & (int)button) == (int)button);
                controller.SetJoystickButton(pressed, buttonIndex, vJoyTypes.StickAndPedals);
                buttonIndex++;
            }
        }
    }
}
