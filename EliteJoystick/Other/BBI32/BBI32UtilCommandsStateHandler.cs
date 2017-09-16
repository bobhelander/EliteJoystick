using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Other.BBI32
{
    public class BBI32UtilCommandsStateHandler : StateHandler
    {
        private ButtonBoxController buttonBoxController;

        public ButtonBoxController ButtonBoxController
        {
            get { return buttonBoxController; }
            set
            {
                buttonBoxController = value;
                if (null != buttonBoxController)
                {
                    buttonBoxController.Controller.ButtonsChanged += Controller_ButtonsChanged;
                }
            }
        }

        private void Controller_ButtonsChanged(object sender, Faz.SideWinderSC.Logic.ButtonStateEventArgs e)
        {
            // Orbit Lines Toggle Off
            if (ButtonBoxController.TestButtonPressed(
                e.PreviousButtonsState, e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button8))
            {
                // Orbit Lines Toggle
                if (ButtonBoxController.SharedState.OrbitLines)
                    ButtonBoxController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.OrbitLinesToggle, 150);

                ButtonBoxController.SharedState.OrbitLines = false;
            }
            // Orbit Lines Toggle On
            if (ButtonBoxController.TestButtonPressed(
                e.PreviousButtonsState, e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button9))
            {
                // Orbit Lines Toggle
                if (false == ButtonBoxController.SharedState.OrbitLines)
                    ButtonBoxController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.OrbitLinesToggle, 150);

                ButtonBoxController.SharedState.OrbitLines = true;
            }

            // HUD Toggle Off
            if (ButtonBoxController.TestButtonPressed(
                e.PreviousButtonsState, e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button10))
            {
                // HUD off  CRTL+ALT+G
                if (ButtonBoxController.SharedState.HeadsUpDisplay)
                    buttonBoxController.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47);

                ButtonBoxController.SharedState.HeadsUpDisplay = false;
            }

            // HUD Toggle On
            if (ButtonBoxController.TestButtonPressed(
                     e.PreviousButtonsState, e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button11))
            {
                // HUD off  CRTL+ALT+G
                if (false == ButtonBoxController.SharedState.HeadsUpDisplay)
                    buttonBoxController.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47);

                ButtonBoxController.SharedState.HeadsUpDisplay = true;
            }
            //if (ButtonBoxController.TestButtonPressed(e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button17))
            //{
            //    // Oculus ASW off  CRTL+KP1
            //    buttonBoxController.SendKeyCombo(new byte[] { 0x80 }, 0x31);
            //}

            if (ButtonBoxController.TestButtonPressed(
                e.PreviousButtonsState, e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button7))
            {
                // Take Picture  ALT-F10
                buttonBoxController.SendKeyCombo(new byte[] { 0x82 }, 0xCB);
            }

            // Camera On/Off
            if (ButtonBoxController.TestButtonPressedOrReleased(
                e.PreviousButtonsState, e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button12))
            {
                if (buttonBoxController.SharedState.CameraActive)
                {
                    // Quit Camera
                    ButtonBoxController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraDisabled, 150);
                }
                else
                {
                    // Start Camera
                    ButtonBoxController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);
                }
                buttonBoxController.SharedState.CameraActive = !buttonBoxController.SharedState.CameraActive;
            }

            // Free Camera On/Off
            if (ButtonBoxController.TestButtonPressedOrReleased(
                e.PreviousButtonsState, e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button12))
            {
                ButtonBoxController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.FreeCameraToggle, 150);
            }

            // ToggleAdvanceMode
            if (ButtonBoxController.TestButtonPressedOrReleased(
                e.PreviousButtonsState, e.ButtonsState, (UInt32)Faz.SideWinderSC.Logic.BBI32Button.Button4))
            {
                ButtonBoxController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraAdvanceModeToggle, 150);
            }

        }
    }
}
