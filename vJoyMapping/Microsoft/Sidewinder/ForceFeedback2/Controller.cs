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
        void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick(devicePath);

            MapControls(joystick);

            joystick.Initialize();
        }

        List<ObserverMapping<States>> observers = new List<ObserverMapping<States>>();

        public void MapControls(Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick ffb2)
        {
            observers.Add(new ObserverMapping<States> { Observer = new Swff2XYJoystick { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new Swff2ZJoystick { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new Swff2SliderJoystick { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new Swff2Hat { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new Swff2ButtonsStateHandler { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new Swff2ClipboardStateHandler { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new Swff2UtilCommandsStateHandler { Controller = this } });

            foreach (var observer in observers)
                observer.Unsubscriber = ffb2.Subscribe(observer.Observer);
        }

        public void Dispose()
        {
            foreach (var observer in observers)
                observer.Unsubscriber?.Dispose();
        }
    }
}
