using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle
{
    public class Controller : Common.Controller
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<IDisposable> Disposables { get; set; }

        void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick(devicePath);
            MapControls(joystick);
            MapLights(joystick);
            joystick.Initialize();
        }

        public void MapControls(Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick ffb2)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                ffb2.Subscribe(x => TmThrottle75Command.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleButtonStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleCameraCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleClearMessages.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleCycleCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleHardpointsCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleHat.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleLandedStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleLandingGearCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleLightsCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleSecondaryFireCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleSilentCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleSliderJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleStateModifier.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleVoiceCommandHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleXYJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => TmThrottleZJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
            };
        }

        public void MapLights(Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick ffb2)
        {
        }

        public void Dispose()
        {
            foreach (var disposable in Disposables)
                disposable?.Dispose();
        }
    }
}
