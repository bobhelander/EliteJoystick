using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping
{
    public static class SwGameButtonStateHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(States value, Controller controller)
        {
            if ((value.Current.Buttons & (byte)Button.ButtonAll) == (byte)Button.ButtonAll)
            {
                controller.SetJoystickButton(true, MappedButtons.VoiceButtonAll, vJoyTypes.Commander);
                return;
            }

            uint buttonIndex = MappedButtons.VoiceButtonAll;
            foreach (Button button in Enum.GetValues(typeof(Button)))
            {
                controller.SetJoystickButton(
                    Reactive.ButtonPressed(value, (uint)button), buttonIndex, vJoyTypes.Commander);
                buttonIndex++;
            }
        }
    }
}
