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
        private Timer timer;
        private bool pressed;

        public long Delay { get; set; }
        public uint vButton { get; set; }

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
            // Flaps Up : Debug Camera
            var button22 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button22;

            if (0 == (e.PreviousButtons & button22) && button22 == (e.Buttons & button22))
            {
                tmThrottleController.SharedState.CameraActive = true;
                if (null == timer)
                    Activate();
                tmThrottleController.VisualState.UpdateMessage("Camera Activated");
            }

            if (button22 == (e.PreviousButtons & button22) && 0 == (e.Buttons & button22))
            {
                tmThrottleController.SharedState.CameraActive = false;
                if (null == timer)
                    Activate();
                tmThrottleController.VisualState.UpdateMessage("Camera Deactivated");
            }
        }

        public void Activate()
        {
            timer = new Timer(new TimerCallback(Action), null, 0, Timeout.Infinite);
        }

        public void Disable()
        {            
            if (null != timer)
            {
                var temp = timer;
                timer = null;
                temp.Dispose();
                tmThrottleController.SetJoystickButton(false, vButton, vJoyTypes.Virtual);
                pressed = false;
            }
        }

        public virtual void Action(object o)
        {
            pressed = !pressed;
            tmThrottleController.SetJoystickButton(pressed, vButton, vJoyTypes.Virtual);

            if (pressed)
            {
                timer.Change(Delay, Timeout.Infinite);
                return;
            }

            Disable();
        }
    }
}
