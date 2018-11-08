using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
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

        public void MapControls(Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick warthog)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                warthog.Subscribe(x => TmThrottle75Command.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleButtonStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleCameraCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleClearMessages.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleCycleCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleHardpointsCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleHat.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleLandedStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleLandingGearCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleLightsCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleSecondaryFireCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleSilentCommand.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleSliderJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleStateModifier.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleVoiceCommandHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleXYJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleZJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
            };
        }

        public void MapLights(Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick warthog)
        {
            // Turn lights on and off
            SharedState.ModeChanged.Subscribe(x => {
                warthog.Lights = (byte)Light.LEDBacklight;
                warthog.LightIntensity = (byte)GetIntensity(x);
                });
        }

        private Intensity GetIntensity(EliteSharedState.Mode mode)
        {
            if (mode == EliteSharedState.Mode.Fighting)
                return Intensity.EXTRA_HIGH;
            if (mode == EliteSharedState.Mode.Travel)
                return Intensity.MED;
            if (mode == EliteSharedState.Mode.Mining)
                return Intensity.EXTRA_LOW;

            return Intensity.OFF;
        }

        public void Dispose()
        {
            foreach (var disposable in Disposables)
                disposable?.Dispose();
        }
    }
}
