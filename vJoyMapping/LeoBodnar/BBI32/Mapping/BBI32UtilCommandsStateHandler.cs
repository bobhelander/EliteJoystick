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

            if (controller.TestButtonDown(value.Current.Buttons, (uint)BBI32Button.Button1))
            {
                // If in Fixed Camera Mode
                ProcessFixed(value, controller);

            }
            else if (controller.TestButtonDown(value.Current.Buttons, (uint)BBI32Button.Button2))
            {
                // If in Free Camera Mode 
                ProcessFree(value, controller);
            }
            else
            {
                ProcessNorm(value, controller);
            }

            ProcessCommon(value, controller);

            //if (ButtonBoxController.TestButtonPressed(e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button17))
            //{
            //    // Oculus ASW off  CRTL+KP1
            //    buttonBoxController.SendKeyCombo(new byte[] { 0x80 }, 0x31);
            //}

            // ToggleAdvanceMode
            //if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button4))
            //{
            //    controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraAdvanceModeToggle, 150);
            //}
        }

        public static void ProcessCommon(States value, Controller controller)
        {
            // Kill process
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button5))
            {
                log.Debug($"Kill process");
                Utils.KillProcess("EliteDangerous64").Wait();
            }

            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button12))
            {
                // Take Picture  ALT-F10
                Task.Run(async () => await controller.SendKeyCombo(new byte[] { 0x82 }, 0xCB).ConfigureAwait(false));
                log.Debug($"Take Picture");
            }
        }

        public static void ProcessNorm(States value, Controller controller)
        {
            // Camera Quit
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button1) ||
                Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button2))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraDisabled, 150);
            }

            // Orbit Lines Toggle On/Off
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button7))
            {
                // Orbit Lines Toggle
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.OrbitLinesToggle, 150);

                controller.SharedState.OrbitLines = !controller.SharedState.OrbitLines;
                log.Debug($"OrbitLines: {controller.SharedState.OrbitLines}");
            }

            // FFS Scan 180 / Rotation Lock
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button8))
            {
                // Rotation Lock Toggle
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LockRotationToggle, 150);
            }

            // Cockpit HUD Toggle On/Off
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button9))
            {
                // CRTL+ALT+G
                Task.Run(async () => await controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47).ConfigureAwait(false));

                log.Debug($"Toggle HeadsUpDisplay");
            }

            // UI Panel Focus  <UIFocus>
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button10))
            {
                // Mapped to BBI32Button10 = vJoy0  Button 30 
            }
        }

        public static void ProcessFixed(States value, Controller controller)
        {
            // Camera On
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button1))
            {
                // Start Camera
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);
            }

            // Camera HUD Toggle On/Off
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button9))
            {
                // J Key
                Task.Run(async () => await controller.SendKeyCombo(new byte[] { }, 0x4A).ConfigureAwait(false));
            }

            // Ship or Camera Control
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button11))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LockToVehicalToggle, 150);
            }
        }

        public static void ProcessFree(States value, Controller controller)
        {
            // Free Camera On
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button2))
            {
                // Start Free Camera without waiting
                StartFreeCamera(controller);
            }

            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button7))
            {
            }

            // Rotation Lock Toggle
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button8))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LockRotationToggle, 150);
            }

            // Camera HUD Toggle On/Off
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button9))
            {
                // H Key
                Task.Run(async () => await controller.SendKeyCombo(new byte[] { }, 0x48).ConfigureAwait(false));
            }

            // World Lock Toggle
            if (Reactive.ButtonPressedOrReleased(value, (uint)BBI32Button.Button10))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LockToWorldToggle, 150);
            }

            // Vanity Camera Next
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button11))
            {
                // Vanity Camera Next
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.NextCamera, 150);
            }
        }

        private static void StartFreeCamera(Controller controller)
        {
            Task.Run(async () =>
            {
                // Start Camera
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);

                // Wait for it
                await Task.Delay(800).ConfigureAwait(false);

                // Toggle Free Camera
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.FreeCameraToggle, 150);

            }).ContinueWith(t =>
            {
                if (t.IsCanceled) log.Error($"StartFreeCamera Canceled");
                else if (t.IsFaulted) log.Error($"StartFreeCamera Exception: {t.Exception}");
                else log.Debug($"StartFreeCamera");
            });
        }
    }
}
