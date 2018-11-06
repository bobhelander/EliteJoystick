using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usb.GameControllers.Interfaces;
using Usb.GameControllers.LeoBodnar.BBI32.Models;
using vJoyMapping.Common;
using vJoyMapping.LeoBodnar.BBI32.Mapping;

namespace vJoyMapping.LeoBodnar.BBI32
{
    public class Controller : Common.Controller, IDisposable
    {
        void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.LeoBodnar.BBI32.Joystick(devicePath);

            MapControls(joystick);

            joystick.Initialize();
        }

        List<ObserverMapping<States>> observers = new List<ObserverMapping<States>>();

        public void MapControls(Usb.GameControllers.LeoBodnar.BBI32.Joystick ffb2)
        {
            observers.Add(new ObserverMapping<States> { Observer = new BBI32ButtonStateHandler { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new BBI32UtilCommandsStateHandler { Controller = this } });

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
