using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usb.GameControllers.Interfaces;
using Usb.GameControllers.CHProducts.ProPedals.Models;
using vJoyMapping.Common;
using vJoyMapping.CHProducts.ProPedals.Mapping;

namespace vJoyMapping.CHProducts.ProPedals
{
    public class Controller : Common.Controller, IDisposable
    {
        void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.CHProducts.ProPedals.Joystick(devicePath);

            MapControls(joystick);

            joystick.Initialize();
        }

        List<ObserverMapping<States>> observers = new List<ObserverMapping<States>>();

        public void MapControls(Usb.GameControllers.CHProducts.ProPedals.Joystick ffb2)
        {
            observers.Add(new ObserverMapping<States> { Observer = new ChPedalsXYR { Controller = this } });

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
