using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usb.GameControllers.Interfaces;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;
using vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2
{
    public class Controller : Common.Controller, IDisposable
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<IDisposable> Disposables { get; set; }

        public void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick(devicePath);

            MapControls(joystick);

            joystick.Initialize();
        }

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick ffb2)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                ffb2.Subscribe(x => Swff2XYJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => Swff2ZJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => Swff2SliderJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => Swff2Hat.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => Swff2ButtonsStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => Swff2ClipboardStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                ffb2.Subscribe(x => Swff2UtilCommandsStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
            };
        }

        public void Dispose()
        {
            foreach (var disposable in Disposables)
                disposable?.Dispose();
        }
    }
}
