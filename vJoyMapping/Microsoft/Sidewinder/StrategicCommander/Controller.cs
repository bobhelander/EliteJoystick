using EliteJoystick.Common;
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
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick(devicePath);

            MapControls(joystick);
            MapLights(joystick);

            joystick.Initialize();

            joystick.ReadInputReportAsync().Wait();
        }

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick swsc)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                swsc.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwCommanderButtonStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                swsc.Subscribe(x => SwCommanderProfileHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                swsc.Subscribe(x => SwCommanderXYZJoystick.Process(x, this), ex => log.Error($"Exception : {ex}")),
                swsc.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwCommanderProgramIds.Process(x, this), ex => log.Error($"Exception : {ex}"))
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
