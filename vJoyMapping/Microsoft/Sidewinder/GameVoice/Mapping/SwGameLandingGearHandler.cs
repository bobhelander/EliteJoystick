using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping
{
    public static class SwGameLandingGearHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static private byte button1 = (byte)Button.Button1;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (0 == (previous.Buttons & button1) && (current.Buttons & button1) == button1)
            {
                // Deployed
                controller.SharedState.ChangeGear(true);
            }
            else if (button1 == (previous.Buttons & button1) && 0 == (current.Buttons & button1))
            {
                // Retracted
                controller.SharedState.ChangeGear(false);
            }
        }
    }
}
