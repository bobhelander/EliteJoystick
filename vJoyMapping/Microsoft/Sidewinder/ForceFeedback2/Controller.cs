using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping;
using Usb.GameControllers.Common;
using Microsoft.Extensions.Logging;
using EliteJoystick.Common.Interfaces;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2
{
    public class Controller : Common.Controller
    {
        public override String Name { get; } = "Force Feedback 2";

        public Controller(
            IKeyboard arduino,
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
                Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick.VendorId,
                Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick.ProductId));

            Logger?.LogDebug($"Added {Name}");
        }

        public void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick(devicePath, Logger);

            MapControls(joystick);

            joystick.Initialize();
        }

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick ffb2)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                ffb2.Subscribe(x => Swff2XYJoystick.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                ffb2.Subscribe(x => Swff2ZJoystick.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                ffb2.Subscribe(x => Swff2SliderJoystick.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                ffb2.Subscribe(x => Swff2Hat.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                ffb2.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => Swff2ButtonsStateHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                ffb2.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => Swff2ClipboardStateHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                ffb2.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => Swff2UtilCommandsStateHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
            };
        }
    }
}
