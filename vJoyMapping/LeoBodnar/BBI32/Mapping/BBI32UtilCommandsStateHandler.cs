using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using Usb.GameControllers.LeoBodnar.BBI32.Models;
using vJoyMapping.Common;

namespace vJoyMapping.LeoBodnar.BBI32.Mapping
{
    public static class BBI32UtilCommandsStateHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(States value, Controller controller)
        {
            // Orbit Lines Toggle On/Off
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button11))
            {
                // Orbit Lines Toggle
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.OrbitLinesToggle, 150);

                controller.SharedState.OrbitLines = !controller.SharedState.OrbitLines;
                log.Debug($"OrbitLines: {controller.SharedState.OrbitLines}");
            }

            // HUD Toggle On/Off
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button12))
            {
                // HUD off  CRTL+ALT+G
                Task.Run(async () => await controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47).ConfigureAwait(false));

                controller.SharedState.HeadsUpDisplay = !controller.SharedState.HeadsUpDisplay;
                log.Debug($"HeadsUpDisplay: {controller.SharedState.HeadsUpDisplay}");
            }

            //if (ButtonBoxController.TestButtonPressed(e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button17))
            //{
            //    // Oculus ASW off  CRTL+KP1
            //    buttonBoxController.SendKeyCombo(new byte[] { 0x80 }, 0x31);
            //}

            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button7))
            {
                // Take Picture  ALT-F10
                Task.Run(async () => await controller.SendKeyCombo(new byte[] { 0x82 }, 0xCB).ConfigureAwait(false));
                log.Debug($"Take Picture ");
            }

            // Camera On/Off
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button1))
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
            //if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button12))
            //{
            //    controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.FreeCameraToggle, 150);
            //}

            // ToggleAdvanceMode
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button4))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraAdvanceModeToggle, 150);
            }

            // Kill process
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button5))
            {
                log.Debug($"Kill process");
                Utils.KillProcess("EliteDangerous64").Wait();
            }
        }
    }
}
