using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleCameraCommand
    {
        // Flaps Up : Debug Camera
        private const UInt32 button22 = (UInt32)Button.Button22;
        private const UInt32 button15 = (UInt32)Button.Button15;

        public static void Process(States value, Controller controller)
        {
            // Camera On/Off
            if (Reactive.ButtonPressedOrReleased(value, button22))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraDisabled, 150);
            }

            // Momentary Camera On/Off
            // Camera turns on so we can switch views quickly
            if (Reactive.ButtonPressed(value, button15))
            {
                // Start Camera
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);
            }
            else if (Reactive.ButtonReleased(value, button15))
            {
                // Quit Camera
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraDisabled, 150);
            }
        }
    }
}
