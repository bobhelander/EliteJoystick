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
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick joystick;

        //Observable.Interval: To create the heartbeat the the button will be pressed on
        private static IObservable<int> timer = Observable.Interval(TimeSpan.FromMilliseconds(2000)).Select(_ => 1);

        public void Initialize(string devicePath)
        {
            joystick = new Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick(devicePath);
            MapControls(joystick);
            //MapLights(joystick);

            // Initialize Voicemeeter and Login
            Disposables.Add(VoiceMeeter.Remote.Initialize().Result);

            joystick.Initialize();

            //WatchChanges();

            UpdateLights();
        }

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick swgv)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                //swgv.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwGameButtonStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                //swgv.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwGameLandingGearHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                swgv.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwGameVoicemeeterHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                //swgv.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => SwGameMuteHandler.Process(x, this), ex => log.Error($"Exception : {ex}"))
            };
        }

        public void WatchChanges()
        {
            Disposables.Add(timer.Subscribe(x =>
            {
                if (VoiceMeeter.Remote.IsParametersDirty() == 1)
                {
                    UpdateLights();
                }
            }));
        }

        public void MapLights(Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick swgv)
        {
            swgv.Lights = 0;
            // Turn lights on and off
            SharedState.GearChanged.Subscribe(x =>
                swgv.Lights = x ? (byte)(swgv.Lights | (byte)Button.Button1) : (byte)(swgv.Lights & ~(byte)Button.Button1));
        }

        public void UpdateLights()
        {
            float button1 = 0;
            float button2 = 0;
            float button3 = 0;
            float button4 = 0;

            VoiceMeeter.Remote.GetParameter("Strip[0].Mute", ref button1);
            VoiceMeeter.Remote.GetParameter("Strip[1].Mute", ref button2);
            VoiceMeeter.Remote.GetParameter("Bus[1].Mute", ref button3);
            VoiceMeeter.Remote.GetParameter("Bus[2].Mute", ref button4);

            byte lights = 0;

            if (button1 == 0) lights = (byte)(lights | (byte)Button.Button1);
            if (button2 == 0) lights = (byte)(lights | (byte)Button.Button2);
            if (button3 == 0) lights = (byte)(lights | (byte)Button.Button3);
            if (button4 == 0) lights = (byte)(lights | (byte)Button.Button4);

            joystick.Lights = lights;
        }

        public void SetLights(byte lights)
        {
            joystick.Lights = lights;
        }
    }
}
