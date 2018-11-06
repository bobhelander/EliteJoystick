using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleCameraCommand : IObserver<States>
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        // Flaps Up : Debug Camera
        static UInt32 button22 = (UInt32)Button.Button22;
        static UInt32 button15 = (UInt32)Button.Button15;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            // Camera On/Off
            if (Controller.TestButtonPressedOrReleased(previous.buttons, current.buttons, button22))
            {
                if (Controller.SharedState.CameraActive)
                {
                    // Quit Camera
                    Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraDisabled, 150);
                    log.Debug("Camera Disabled");
                }
                else
                {
                    // Start Camera
                    Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);
                    log.Debug("Camera Enabled");
                }
                Controller.SharedState.CameraActive = !Controller.SharedState.CameraActive;
            }

            // Momentary Camera On/Off
            // Camera turns on so we can switch views quickly
            if (Controller.TestButtonPressed(previous.buttons, current.buttons, button15))
            {
                // Start Camera
                Controller.CallActivateButton(vJoyTypes.Virtual, 16, 150);
            }
            else if (Controller.TestButtonReleased(previous.buttons, current.buttons, button15))
            {
                // Quit Camera
                Controller.CallActivateButton(vJoyTypes.Virtual, 18, 150);
            }
        }
    }
}
