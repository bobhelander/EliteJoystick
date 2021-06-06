using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleShieldCellCommand
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Observable.Interval: To create the heartbeat the the button will be pressed on
        private static IObservable<int> timer = Observable.Interval(TimeSpan.FromMilliseconds(1000)).Select(_ => 1);
        private static int counter = 0;

        // This will be non null while the action is running
        private static IDisposable fireShieldCells = null;

        static readonly UInt32 BoatForward = (UInt32)Button.Button09;

        /*
        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, BoatForward))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.ShieldCell, 200);
            }
        }
        */

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, BoatForward) &&
                controller.SharedState.CurrentMode == EliteSharedState.Mode.Fighting)
            {
                if (null == fireShieldCells)
                {
                    counter = 0;
                    fireShieldCells = timer.Subscribe(x =>
                    {
                        if (counter % 12 == 0)  // 0, 12000, 240000
                        {
                            log.Debug($"Fire shield cells");
                            controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.ShieldCell, 200);
                        }
                        if (counter == 6 || counter == 21)  // 6000, 21000
                        {
                            log.Debug($"Fire heat sink");
                            controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.HeatSink, 200);
                        }
                        counter++;
                    });                    
                }
            }
            else
            {
                if (null != fireShieldCells)
                {
                    fireShieldCells.Dispose();
                    fireShieldCells = null;
                    log.Debug($"Fire shield cells: Stopped");
                }
            }
        }
    }
}
