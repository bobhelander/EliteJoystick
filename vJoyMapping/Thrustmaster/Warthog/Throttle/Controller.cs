using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle
{
    public class Controller : Common.Controller
    {
        public override String Name { get; } = "Warthog Throttle";
        public Controller(
            IKeyboard arduino,
            EliteSharedState eliteSharedState,
            ISettings settings,
            IVirtualJoysticks virtualJoysticks,
            ILogger<Controller> log)
        {
            Keyboard = arduino;
            SharedState = eliteSharedState;
            Settings = settings;
            VirtualJoysticks = virtualJoysticks;
            Logger = log;

            Initialize(Controller.GetDevicePath(
                Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick.VendorId,
                Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick.ProductId));

            Logger?.LogDebug($"Added {Name}");
        }

        public void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick(devicePath, Logger);
            MapControls(joystick);
            MapLights(joystick);
            joystick.Initialize();
        }

        public void MapControls(Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick warthog)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottle75Command.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleButtonStateHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleCameraCommand.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleClearMessages.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleCycleCommand.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleHardpointsCommand.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleHat.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleLandedStateHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleLandingGearCommand.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleLightsCommand.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleHeatSinkCommand.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleShieldCellCommand.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleSecondaryFireCommand.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                //warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleSilentCommand.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleSliderJoystick.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleStateModifier.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                //warthog.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => TmThrottleVoiceCommandHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleXYJoystick.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                warthog.Subscribe(x => TmThrottleZJoystick.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
            };
        }

        public void MapLights(Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick warthog)
        {
            // Turn lights on and off
            SharedState.ModeChanged.Subscribe(x =>
            {
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
    }
}
