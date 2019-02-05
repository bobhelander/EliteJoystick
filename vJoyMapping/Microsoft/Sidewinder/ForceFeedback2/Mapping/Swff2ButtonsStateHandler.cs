using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2ButtonsStateHandler
    {
        public static void Process(States value, Controller controller)
        {
            controller.SetJoystickButtons(value.Current.Buttons, vJoyTypes.StickAndPedals, MappedButtons.ForceFeedbackButtonMask);
            //uint buttonIndex = MappedButtons.ForceFeedback2Trigger;
            //foreach (Button button in Enum.GetValues(typeof(Button)))
            //{
            //    controller.SetJoystickButton(
            //        Reactive.ButtonDown(value, (uint)button), buttonIndex, vJoyTypes.StickAndPedals);
            //    buttonIndex++;
            //}
        }
    }
}
