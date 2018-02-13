using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleCycleCommand : StateHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                    tmThrottleController.Controller.SwitchState += async (s, e) =>
                        await Task.Run(() => ControllerSwitchState(s, e));
                }
            }
        }

        static private UInt32 button07 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button07;

        private void ControllerSwitchState(object sender, Faz.SideWinderSC.Logic.TmThrottleSwitchEventArgs e)
        {
            if ((e.Buttons & button07) == button07 &&
                tmThrottleController.SharedState.CurrentMode == EliteSharedState.Mode.Fighting)
            {
                if (null == timer)
                    Activate();
            }
            else
            {
                if (null != timer)
                {
                    Disable();
                }
            }
        }

        public void Activate()
        {
            //tmThrottleController.VisualState.UpdateMessage("Cycle Subsystem: Running");
            timer = new Timer(new TimerCallback(Action), null, 0, Timeout.Infinite);
            log.Debug($"Cycle Subsystem: Running");
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
            log.Debug($"Cycle Subsystem: Stopped");
        }

        public virtual void Action(object o)
        {
            pressed = !pressed;
            tmThrottleController.SetJoystickButton(pressed, vButton, vJoyTypes.Virtual);
            timer.Change(Delay, Timeout.Infinite);
        }
    }
}
