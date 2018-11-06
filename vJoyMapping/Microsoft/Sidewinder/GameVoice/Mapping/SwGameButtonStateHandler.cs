using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping
{
    public class SwGameButtonStateHandler : IObserver<States>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        static private byte button1 = (byte)Button.Button1;


        public void OnNext(States value)
        {
            var current = value.Current as State;
 
            uint buttonIndex = 1;
            foreach (Button button in Enum.GetValues(typeof(Button)))
            {
                bool pressed = ((current.Buttons & (byte)button) == (byte)button);
                Controller.SetJoystickButton(pressed, buttonIndex, vJoyTypes.Voice);
                buttonIndex++;
            }
        }
    }
}
