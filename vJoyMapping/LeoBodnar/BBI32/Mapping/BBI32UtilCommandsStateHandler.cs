using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.LeoBodnar.BBI32.Models;
using vJoyMapping.Common;

namespace vJoyMapping.LeoBodnar.BBI32.Mapping
{
    public class BBI32UtilCommandsStateHandler : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            // Orbit Lines Toggle Off
            if (Controller.TestButtonPressed(
                previous.Buttons, current.Buttons, (UInt32)BBI32Button.Button8))
            {
                // Orbit Lines Toggle
                if (Controller.SharedState.OrbitLines)
                    Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.OrbitLinesToggle, 150);

                Controller.SharedState.OrbitLines = false;
            }

            // Orbit Lines Toggle On
            if (Controller.TestButtonPressed(
                previous.Buttons, current.Buttons, (UInt32)BBI32Button.Button9))
            {
                // Orbit Lines Toggle
                if (false == Controller.SharedState.OrbitLines)
                    Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.OrbitLinesToggle, 150);

                Controller.SharedState.OrbitLines = true;
            }

            // HUD Toggle Off
            if (Controller.TestButtonPressed(
                previous.Buttons, current.Buttons, (UInt32)BBI32Button.Button10))
            {
                // HUD off  CRTL+ALT+G
                if (Controller.SharedState.HeadsUpDisplay)
                    Controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47);

                Controller.SharedState.HeadsUpDisplay = false;
            }

            // HUD Toggle On
            if (Controller.TestButtonPressed(
                previous.Buttons, current.Buttons, (UInt32)BBI32Button.Button11))
            {
                // HUD off  CRTL+ALT+G
                if (false == Controller.SharedState.HeadsUpDisplay)
                    Controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47);

                Controller.SharedState.HeadsUpDisplay = true;
            }
            //if (ButtonBoxController.TestButtonPressed(e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button17))
            //{
            //    // Oculus ASW off  CRTL+KP1
            //    buttonBoxController.SendKeyCombo(new byte[] { 0x80 }, 0x31);
            //}

            if (Controller.TestButtonPressed(
                previous.Buttons, current.Buttons, (UInt32)BBI32Button.Button7))
            {
                // Take Picture  ALT-F10
                Controller.SendKeyCombo(new byte[] { 0x82 }, 0xCB);
            }

            // Camera On/Off
            if (Controller.TestButtonPressedOrReleased(
                previous.Buttons, current.Buttons, (UInt32)BBI32Button.Button12))
            {
                if (Controller.SharedState.CameraActive)
                {
                    // Quit Camera
                    Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraDisabled, 150);
                }
                else
                {
                    // Start Camera
                    Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);
                }
                Controller.SharedState.CameraActive = !Controller.SharedState.CameraActive;
            }

            // Free Camera On/Off
            if (Controller.TestButtonPressedOrReleased(
                previous.Buttons, current.Buttons, (UInt32)BBI32Button.Button12))
            {
                Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.FreeCameraToggle, 150);
            }

            // ToggleAdvanceMode
            if (Controller.TestButtonPressedOrReleased(
                previous.Buttons, current.Buttons, (UInt32)BBI32Button.Button4))
            {
                Controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraAdvanceModeToggle, 150);
            }

            // Kill process
            if (Controller.TestButtonPressed(
                previous.Buttons, current.Buttons, (UInt32)BBI32Button.Button2))
            {
                Utils.KillProcess("EliteDangerous64").Wait();
            }
        }
    }
}
