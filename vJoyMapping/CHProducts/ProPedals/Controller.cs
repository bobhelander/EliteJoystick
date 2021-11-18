using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using vJoyMapping.CHProducts.ProPedals.Mapping;
using Microsoft.Extensions.Logging;
using EliteJoystick.Common.Interfaces;

namespace vJoyMapping.CHProducts.ProPedals
{
    public class Controller : Common.Controller
    {
        public override String Name { get; } = "Pro Pedals";

        public Controller(
            IKeyboard arduino,
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

            var altProductId = false;

            retry:

            try
            {
                var productId = altProductId ?
                    Usb.GameControllers.CHProducts.ProPedals.JoystickMSDriver.ProductId :
                    Usb.GameControllers.CHProducts.ProPedals.Joystick.ProductId;

                Initialize(Controller.GetDevicePath(
                    Usb.GameControllers.CHProducts.ProPedals.Joystick.VendorId,
                    productId), altProductId);

                Logger?.LogDebug($"Added {Name}");
            }
            catch (Exception _)
            {
                if (altProductId == false)
                {
                    altProductId = true;
                    goto retry;
                }

                throw;
            }
        }

        public void Initialize(string devicePath, bool usingMsDriver)
        {
            if (usingMsDriver)
            {
                var joystick = new Usb.GameControllers.CHProducts.ProPedals.JoystickMSDriver(devicePath, Logger);

                MapControls(joystick);

                joystick.Initialize();
            }
            else
            {
                var joystick = new Usb.GameControllers.CHProducts.ProPedals.Joystick(devicePath, Logger);

                MapControls(joystick);

                joystick.Initialize();
            }
        }

        public void MapControls(Usb.GameControllers.CHProducts.ProPedals.Joystick proPedals)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                proPedals.Subscribe(x => ChPedalsXYR.Process(x, this), ex => Logger.LogError($"Exception : {ex}"))
            };
        }

        public void MapControls(Usb.GameControllers.CHProducts.ProPedals.JoystickMSDriver proPedals)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                proPedals.Subscribe(x => ChPedalsXYR.Process(x, this), ex => Logger.LogError($"Exception : {ex}"))
            };
        }
    }
}
