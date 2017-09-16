using EliteJoystick.Thrustmaster.Warthog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystick
{
    public class TmThrottleCameraCommand : StateHandler
    {
        // Flaps Up : Debug Camera
        static UInt32 button22 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button22;
        static UInt32 button15 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button15;

        public Faz.SideWinderSC.Logic.TmThrottleButton ButtonId { get; set; }

        private TmThrottleController tmThrottleController;

        public TmThrottleController TmThrottleController
        {
            get { return tmThrottleController; }
            set
            {
                tmThrottleController = value;
                if (null != tmThrottleController)
                {
                    tmThrottleController.Controller.SwitchState += Controller_SwitchState;
                }
            }
        }

        private void Controller_SwitchState(object sender, Faz.SideWinderSC.Logic.TmThrottleSwitchEventArgs e)
        {
            // Camera On/Off
            if (tmThrottleController.TestButtonPressedOrReleased(e.PreviousButtons, e.Buttons, button22))
            {
                if (tmThrottleController.SharedState.CameraActive)
                {
                    // Quit Camera
                    tmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraDisabled, 150);
                    tmThrottleController.VisualState.UpdateMessage("Camera Activated");
                }
                else
                {
                    // Start Camera
                    tmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);
                    tmThrottleController.VisualState.UpdateMessage("Camera Activated");
                }
                tmThrottleController.SharedState.CameraActive = !tmThrottleController.SharedState.CameraActive;
            }

            // Momentary Camera On/Off
            // Camera turns on so we can switch views quickly
            if (tmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, button15))
            {
                // Start Camera
                tmThrottleController.CallActivateButton(vJoyTypes.Virtual, 16, 150);
            }
            else if (tmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, button15))
            {
                // Quit Camera
                tmThrottleController.CallActivateButton(vJoyTypes.Virtual, 18, 150);
            }
        }
    }
}
