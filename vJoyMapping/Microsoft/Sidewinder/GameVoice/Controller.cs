using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice
{
    public class Controller : Common.Controller, IDisposable
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<IDisposable> Disposables { get; set; }

        public void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick(devicePath);
            MapControls(joystick);
            MapLights(joystick);
            joystick.Initialize();
        }

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick swgv)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                swgv.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwGameButtonStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                swgv.Subscribe(x => SwGameLandingGearHandler.Process(x, this), ex => log.Error($"Exception : {ex}"))
            };
        }

        public void MapLights(Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick swgv)
        {
            swgv.Lights = 0;
            // Turn lights on and off
            SharedState.GearChanged.Subscribe(x =>
                swgv.Lights = x ? (byte)(swgv.Lights | (byte)Button.Button1) : (byte)(swgv.Lights & ~(byte)Button.Button1));
        }

        public void Dispose()
        {
            foreach (var disposable in Disposables)
                disposable?.Dispose();
        }
    }
}
