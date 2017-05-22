using EliteJoystick.Thrustmaster.Warthog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystick
{
    public class TmThrottleCycleCommand : StateHandler
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
            if ((e.Buttons & (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button07) != 0 &&
                tmThrottleController.SharedState.CurrentMode == EliteSharedState.Mode.Fighting)
            {
                if (null == timer)
                    Activate();
                tmThrottleController.VisualState.UpdateMessage("Cycle Subsystem: Running");
            }
            else
            {
                if (null != timer)
                {
                    Disable();
                    tmThrottleController.VisualState.UpdateMessage("Cycle Subsystem: Done");
                }
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
            timer.Change(Delay, Timeout.Infinite);
        }
    }
}
