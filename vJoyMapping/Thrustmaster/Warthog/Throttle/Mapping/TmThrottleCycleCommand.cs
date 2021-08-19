using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;
using System.Reactive.Linq;
using Usb.GameControllers.Common;
using Microsoft.Extensions.Logging;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleCycleCommand
    {
        //Observable.Interval: To create the heartbeat the the button will be pressed on
        private static readonly IObservable<int> timer = Observable.Interval(TimeSpan.FromMilliseconds(500)).Select(_ => 1);

        // This will be non null while the action is running
        private static IDisposable cycleSubsystems = null;

        private const UInt32 SpeedbrakeForward = (UInt32)Button.Button07;

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
                        controller.Logger.LogDebug("Cycle Subsystem: Next");
                    });
                    controller.Logger.LogDebug("Cycle Subsystem: Running");
                }
            }
            else
            {
                if (null != cycleSubsystems)
                {
                    cycleSubsystems.Dispose();
                    cycleSubsystems = null;
                    controller.Logger.LogDebug("Cycle Subsystem: Stopped");
                }
            }
        }
    }
}
