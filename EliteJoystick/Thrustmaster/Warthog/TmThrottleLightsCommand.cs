using EliteJoystick.Thrustmaster.Warthog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystick
{
    public class TmThrottleLightsCommand : StateHandler
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
            // Fuel Left : Lights
            var button = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button16;

            if (0 == (e.PreviousButtons & button) && button == (e.Buttons & button))
            {
                if (null == timer)
                    Activate();
                tmThrottleController.VisualState.UpdateMessage("Lights On");
            }

            if (button == (e.PreviousButtons & button) && 0 == (e.Buttons & button))
            {
                if (null == timer)
                    Activate();
                tmThrottleController.VisualState.UpdateMessage("Lights Off");
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
