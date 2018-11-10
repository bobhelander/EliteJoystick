using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping
{
    public static class SwGameButtonStateHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (current.Buttons == previous.Buttons)
                return; // No Change

            if ((current.Buttons & (byte)Button.ButtonAll) == (byte)Button.ButtonAll)
            {
                controller.SetJoystickButton(true, MappedButtons.VoiceButtonAll, vJoyTypes.Commander);
                return;
            }

            uint buttonIndex = MappedButtons.VoiceButtonAll;
            foreach (Button button in Enum.GetValues(typeof(Button)))
            {
                bool pressed = ((current.Buttons & (byte)button) == (byte)button);
                controller.SetJoystickButton(pressed, buttonIndex, vJoyTypes.Commander);
                buttonIndex++;
            }
        }
    }
}
