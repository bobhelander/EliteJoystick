using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping
{
    public class SwGameLandingGearHandler : IObserver<States>
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
            var previous = value.Previous as State;

            if (0 == (previous.Buttons & button1) && (current.Buttons & button1) == button1)
            {
                Controller.SharedState.ChangeGear(true);
                // On
                //Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LandingGearToggle, 200);
                //log.Debug($"Virtual: Landing Gear: Deployed");
            }
            else if (button1 == (previous.Buttons & button1) && 0 == (current.Buttons & button1))
            {
                // Off
                Controller.SharedState.ChangeGear(false);
                // Off
                //Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LandingGearToggle, 200);
                //log.Debug($"Virtual: Landing Gear: Retracted");
            }
        }
    }
}
