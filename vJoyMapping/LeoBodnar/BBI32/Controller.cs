﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Usb.GameControllers.Common;
using vJoyMapping.LeoBodnar.BBI32.Mapping;

namespace vJoyMapping.LeoBodnar.BBI32
{
    public class Controller : Common.Controller
    {
        public void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.LeoBodnar.BBI32.Joystick(devicePath, Logger);

            MapControls(joystick);
            joystick.Initialize();
        }

        public void MapControls(Usb.GameControllers.LeoBodnar.BBI32.Joystick bbi32)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                bbi32.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => BBI32ButtonStateHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                bbi32.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => BBI32UtilCommandsStateHandler.Process(x, this), ex => Logger.LogError($"Exception : {ex}")),
                //bbi32.Where(x => Reactive.ButtonsChanged(x)).Subscribe(x => BBI32VoicemeeterHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
            };
        }
    }
}
