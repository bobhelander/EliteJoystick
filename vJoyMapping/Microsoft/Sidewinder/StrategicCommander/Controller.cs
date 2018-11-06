using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usb.GameControllers.Interfaces;
using Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Models;
using vJoyMapping.Common;
using vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Mapping;

namespace vJoyMapping.Microsoft.Sidewinder.StrategicCommander
{
    public class Controller : Common.Controller, IDisposable
    {
        void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick(devicePath);

            MapControls(joystick);

            joystick.Initialize();
        }

        List<ObserverMapping<States>> observers = new List<ObserverMapping<States>>();

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick swsc)
        {
            observers.Add(new ObserverMapping<States> { Observer = new SwCommanderButtonStateHandler { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new SwCommanderProfileHandler { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new SwCommanderXYZJoystick { Controller = this } });
            
            foreach (var observer in observers)
                observer.Unsubscriber = swsc.Subscribe(observer.Observer);
        }

        public void Dispose()
        {
            foreach (var observer in observers)
                observer.Unsubscriber?.Dispose();
        }
    }
}
