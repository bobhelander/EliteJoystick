using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.LeoBodnar.BBI32.Models;
using vJoyMapping.Common;

namespace vJoyMapping.LeoBodnar.BBI32.Mapping
{
    public static class BBI32UtilCommandsStateHandler
    {
        public static void Process(States value, Controller controller)
        {
            // Orbit Lines Toggle Off
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button8))
            {
                // Orbit Lines Toggle
                if (controller.SharedState.OrbitLines)
                    controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.OrbitLinesToggle, 150);

                controller.SharedState.OrbitLines = false;
            }

            // Orbit Lines Toggle On
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button9))
            {
                // Orbit Lines Toggle
                if (false == controller.SharedState.OrbitLines)
                    controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.OrbitLinesToggle, 150);

                controller.SharedState.OrbitLines = true;
            }

            // HUD Toggle Off
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button10))
            {
                // HUD off  CRTL+ALT+G
                if (controller.SharedState.HeadsUpDisplay)
                    controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47);

                controller.SharedState.HeadsUpDisplay = false;
            }

            // HUD Toggle On
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button11))
            {
                // HUD off  CRTL+ALT+G
                if (false == controller.SharedState.HeadsUpDisplay)
                    controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47);

                controller.SharedState.HeadsUpDisplay = true;
            }
            //if (ButtonBoxController.TestButtonPressed(e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button17))
            //{
            //    // Oculus ASW off  CRTL+KP1
            //    buttonBoxController.SendKeyCombo(new byte[] { 0x80 }, 0x31);
            //}

            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button7))
            {
                // Take Picture  ALT-F10
                controller.SendKeyCombo(new byte[] { 0x82 }, 0xCB);
            }

            // Camera On/Off
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button12))
            {
                if (controller.SharedState.CameraActive)
                {
                    // Quit Camera
                    controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraDisabled, 150);
                }
                else
                {
                    // Start Camera
                    controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);
                }
                controller.SharedState.CameraActive = !controller.SharedState.CameraActive;
            }

            // Free Camera On/Off
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button12))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.FreeCameraToggle, 150);
            }

            // ToggleAdvanceMode
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button4))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraAdvanceModeToggle, 150);
            }

            // Kill process
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button2))
            {
                Utils.KillProcess("EliteDangerous64").Wait();
            }
        }
    }
}
