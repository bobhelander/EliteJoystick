﻿using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleCameraCommand
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Flaps Up : Debug Camera
        static UInt32 button22 = (UInt32)Button.Button22;
        static UInt32 button15 = (UInt32)Button.Button15;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            // Camera On/Off
            if (controller.TestButtonPressedOrReleased(previous.buttons, current.buttons, button22))
            {
                if (controller.SharedState.CameraActive)
                {
                    // Quit Camera
                    controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraDisabled, 150);
                    log.Debug("Camera Disabled");
                }
                else
                {
                    // Start Camera
                    controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);
                    log.Debug("Camera Enabled");
                }
                controller.SharedState.CameraActive = !controller.SharedState.CameraActive;
            }

            // Momentary Camera On/Off
            // Camera turns on so we can switch views quickly
            if (controller.TestButtonPressed(previous.buttons, current.buttons, button15))
            {
                // Start Camera
                controller.CallActivateButton(vJoyTypes.Virtual, 16, 150);
            }
            else if (controller.TestButtonReleased(previous.buttons, current.buttons, button15))
            {
                // Quit Camera
                controller.CallActivateButton(vJoyTypes.Virtual, 18, 150);
            }
        }
    }
}
