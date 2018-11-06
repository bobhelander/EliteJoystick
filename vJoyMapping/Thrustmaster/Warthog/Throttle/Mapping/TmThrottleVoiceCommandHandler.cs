using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleVoiceCommandHandler : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        static UInt32 MIC = (UInt32)Button.Button02;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (Controller.TestButtonPressed(previous.buttons, current.buttons, MIC))
            {
                Controller.SharedState.Mute = true;
                Controller.DepressKey(0xC3);  // KEY_F2    
            }
            if (Controller.TestButtonReleased(previous.buttons, current.buttons, MIC))
            {
                Controller.SharedState.Mute = false;
                Controller.ReleaseKey(0xC3);  // KEY_F2
            }
        }
    }
}
