using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usb.GameControllers.Interfaces;
using Usb.GameControllers.CHProducts.ProPedals.Models;
using vJoyMapping.Common;
using vJoyMapping.CHProducts.ProPedals.Mapping;
using Microsoft.Extensions.Logging;

namespace vJoyMapping.CHProducts.ProPedals
{
    public class Controller : Common.Controller
    {
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
