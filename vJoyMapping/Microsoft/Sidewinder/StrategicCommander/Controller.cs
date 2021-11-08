using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Interfaces;
using Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Models;
using vJoyMapping.Common;
using vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Mapping;

namespace vJoyMapping.Microsoft.Sidewinder.StrategicCommander
{
    public class Controller : Common.Controller
    {
        public override String Name { get; } = "Strategic Commander";

        public Controller(
            IArduino arduino,
            EliteSharedState eliteSharedState,
            ISettings settings,
            IVirtualJoysticks virtualJoysticks,
            ILogger<Controller> log)
        {
            Arduino = arduino;
            SharedState = eliteSharedState;
            Settings = settings;
            VirtualJoysticks = virtualJoysticks;
            Logger = log;

            Initialize(Controller.GetDevicePath(
                Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick.VendorId,
                Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick.ProductId));

            Logger?.LogDebug($"Added {Name}");
        }

        public void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick(devicePath, Logger);

            MapControls(joystick);
            MapLights(joystick);

            joystick.Initialize();

            joystick.ReadInputReportAsync().Wait();
        }

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick swsc)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                swsc.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwCommanderButtonStateHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                swsc.Subscribe(x => SwCommanderProfileHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                swsc.Subscribe(x => SwCommanderXYZJoystick.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                swsc.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwCommanderProgramIds.Process(x, this), ex => Logger.LogError($"Exception : {ex}"))
            };
        }

        public void MapLights(Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick swsc)
        {
            // Turn lights on and off
            SharedState.ModeChanged.Subscribe(x => swsc.SetLights(GetLights(x)));
        }

        private IEnumerable<Light> GetLights(EliteSharedState.Mode mode)
        {
            if (mode == EliteSharedState.Mode.Fighting)
                return new Light[] { Light.Button1 };
            if (mode == EliteSharedState.Mode.Travel)
                return new Light[] { Light.Button2 };
            if (mode == EliteSharedState.Mode.Mining)
                return new Light[] { Light.Button3 };

            return new Light[] { };
        }
    }
}
