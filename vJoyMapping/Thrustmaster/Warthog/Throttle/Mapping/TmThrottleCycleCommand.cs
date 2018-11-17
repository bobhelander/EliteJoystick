using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;
using System.Reactive.Linq;
using Usb.GameControllers.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleCycleCommand
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Observable.Interval: To create the heartbeat the the button will be pressed on
        private static IObservable<int> timer = Observable.Interval(TimeSpan.FromMilliseconds(500)).Select(_ => 1);

        // This will be non null while the action is running
        private static IDisposable cycleSubsystems = null;

        private static readonly UInt32 SpeedbrakeForward = (UInt32)Button.Button07;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, SpeedbrakeForward) &&
                controller.SharedState.CurrentMode == EliteSharedState.Mode.Fighting)
            {
                if (null == cycleSubsystems)
                {
                    cycleSubsystems = timer.Subscribe(x =>
                    {
                        controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CycleSubsystem, 200);
                        log.Debug($"Cycle Subsystem: Next");
                    });
                    log.Debug($"Cycle Subsystem: Running");
                }
            }
            else
            {
                if (null != cycleSubsystems)
                {
                    cycleSubsystems.Dispose();
                    cycleSubsystems = null;
                    log.Debug($"Cycle Subsystem: Stopped");
                }
            }
        }
    }
}
