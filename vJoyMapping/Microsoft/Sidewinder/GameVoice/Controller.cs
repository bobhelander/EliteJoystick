using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
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
    public class Controller : Common.Controller
    {
        public override String Name { get; } = "Game Voice";

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
                Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick.VendorId,
                Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick.ProductId));

            Logger?.LogDebug($"Added {Name}");
        }

        private Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick joystick;

        //Observable.Interval: To create the heartbeat the the button will be pressed on
        private static readonly IObservable<int> timer = Observable.Interval(TimeSpan.FromMilliseconds(2000)).Select(_ => 1);

        public void Initialize(string devicePath)
        {
            joystick = new Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick(devicePath, Logger);
            MapControls(joystick);
            //MapLights(joystick);

            // Initialize Voicemeeter and Login
            Disposables.Add(VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterBanana).Result);

            joystick.Initialize();

            //WatchChanges();

            SwGameVoicemeeterHandler.UpdateLights(joystick);
        }

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick swgv)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                //swgv.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwGameButtonStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                //swgv.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwGameLandingGearHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                swgv.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwGameVoicemeeterHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                //swgv.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwGameMuteHandler.Process(x, this), ex => log.Error($"Exception : {ex}"))
            };
        }

        public void WatchChanges()
        {
            Disposables.Add(timer.Subscribe(x =>
            {
                //if (VoiceMeeter.Remote.IsParametersDirty() == 1)
                //{
                //    UpdateLights();
                //}
            }));
        }

        public void MapLights(Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick swgv)
        {
            swgv.Lights = 0;
            // Turn lights on and off
            SharedState.GearChanged.Subscribe(x =>
                swgv.Lights = x ? (byte)(swgv.Lights | (byte)Button.Button1) : (byte)(swgv.Lights & ~(byte)Button.Button1));
        }

        public void SetLights(byte lights)
        {
            joystick.Lights = lights;
        }
    }
}
