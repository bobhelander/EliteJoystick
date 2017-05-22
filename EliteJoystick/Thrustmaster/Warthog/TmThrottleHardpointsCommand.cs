using EliteJoystick.Thrustmaster.Warthog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystick
{
    public class TmThrottleHardpointsCommand : StateHandler
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
            var button19 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button19;
            if (tmThrottleController.SharedState.HardpointsDeployed == false &&
                tmThrottleController.SharedState.CurrentMode == EliteSharedState.Mode.Fighting &&
                ((e.Buttons & button19) == button19))
            {
                tmThrottleController.SharedState.HardpointsDeployed = true;
                if (null == timer)
                    Activate();
                tmThrottleController.VisualState.UpdateMessage("Hardpoints Deployed");
            }

            if (tmThrottleController.SharedState.HardpointsDeployed == true &&
                (e.Buttons & button19) == 0)
            {
                tmThrottleController.SharedState.HardpointsDeployed = false;

                if (null == timer)
                    Activate();
                tmThrottleController.VisualState.UpdateMessage("Hardpoints Stowed");
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
