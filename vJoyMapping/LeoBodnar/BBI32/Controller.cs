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
    public class Controller : Common.Controller
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.LeoBodnar.BBI32.Joystick(devicePath);

            MapControls(joystick);

            joystick.Initialize();
        }

        public void MapControls(Usb.GameControllers.LeoBodnar.BBI32.Joystick bbi32)
        {
            // Add in the mappings
            bbi32.Subscribe(x => BBI32ButtonStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}"));
            bbi32.Subscribe(x => BBI32UtilCommandsStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}"));
        }
    }
}
