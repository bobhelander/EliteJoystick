using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.LeoBodnar.BBI32.Models;
using vJoyMapping.Common;

namespace vJoyMapping.LeoBodnar.BBI32.Mapping
{
    public static class BBI32ButtonStateHandler
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (current.Buttons == previous.Buttons)
                return; // No Change

            // Buttons 21 - 32 On the Commander controller
            uint buttonIndex = MappedButtons.BBI32Button1;
            foreach (UInt32 button in Enum.GetValues(typeof(BBI32Button)))
            {
                bool pressed = controller.TestButtonDown(current.Buttons, button);
                controller.SetJoystickButton(pressed, buttonIndex, vJoyTypes.StickAndPedals);
                buttonIndex++;
            }
        }
    }
}
