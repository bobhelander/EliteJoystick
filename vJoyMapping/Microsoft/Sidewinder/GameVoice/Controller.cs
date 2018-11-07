using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usb.GameControllers.Interfaces;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;
using vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice
{
    public class Controller : Common.Controller, IDisposable
    {
        void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick(devicePath);

            MapControls(joystick);

            joystick.Initialize();
        }

        List<ObserverMapping<States>> observers = new List<ObserverMapping<States>>();

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick swgv)
        {
            observers.Add(new ObserverMapping<States> { Observer = new SwGameButtonStateHandler { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new SwGameLandingGearHandler { Controller = this } });

            foreach (var observer in observers)
                observer.Unsubscriber = swgv.Subscribe(observer.Observer);

            // Turn lights on and off
            SharedState.GearChanged.Subscribe(x => 
                swgv.Lights = x ? (byte)(swgv.Lights | (byte)Button.Button1) : (byte)(swgv.Lights | ~(byte)Button.Button1));
        }

        public void Dispose()
        {
            foreach (var observer in observers)
                observer.Unsubscriber?.Dispose();
        }
    }
}
