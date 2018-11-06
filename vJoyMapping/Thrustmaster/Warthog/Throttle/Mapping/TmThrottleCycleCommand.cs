using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleCycleCommand : IObserver<States>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Timer timer;
        private bool pressed;

        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        private static readonly UInt32 SpeedbrakeForward = (UInt32)Button.Button07;


        public void OnNext(States value)
        {
            var current = value.Current as State;

            if ((current.buttons & SpeedbrakeForward) == SpeedbrakeForward &&
                Controller.SharedState.CurrentMode == EliteSharedState.Mode.Fighting)
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
                Controller.SetJoystickButton(false, MappedButtons.CycleSubsystem, vJoyTypes.Virtual);
                pressed = false;
            }
            log.Debug($"Cycle Subsystem: Stopped");
        }

        public virtual void Action(object o)
        {
            pressed = !pressed;
            Controller.SetJoystickButton(pressed, MappedButtons.CycleSubsystem, vJoyTypes.Virtual);
            timer.Change(250, Timeout.Infinite);
        }
    }
}
