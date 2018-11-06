using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usb.GameControllers.Interfaces;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;
using vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle
{
    public class Controller : Common.Controller, IDisposable
    {
        void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick(devicePath);

            MapControls(joystick);

            joystick.Initialize();
        }

        List<ObserverMapping<States>> observers = new List<ObserverMapping<States>>();

        public void MapControls(Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick ffb2)
        {
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottle75Command { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleButtonStateHandler { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleCameraCommand { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleClearMessages { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleCycleCommand { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleHardpointsCommand { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleHat { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleLandedStateHandler { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleLandingGearCommand { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleLightsCommand { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleSecondaryFireCommand { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleSilentCommand { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleSliderJoystick { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleStateModifier { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleVoiceCommandHandler { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleXYJoystick { Controller = this } });
            observers.Add(new ObserverMapping<States> { Observer = new TmThrottleZJoystick { Controller = this } });

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
