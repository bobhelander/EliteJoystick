using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Mapping
{
    public static class SwCommanderButtonStateHandler
    {
        public static void Process(States value, Controller controller)
        {
            controller.SetJoystickButtons(value.Current.Buttons, vJoyTypes.Commander, MappedButtons.CommanderButtonMask);

            //uint buttonIndex = MappedButtons.CommanderButton1;
            //foreach (Button button in Enum.GetValues(typeof(Button)))
            //{
            //    controller.SetJoystickButton(
            //        Reactive.ButtonDown(value, (uint)button), buttonIndex, vJoyTypes.Commander);
            //    buttonIndex++;
            //}
        }
    }
}
