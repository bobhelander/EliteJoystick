using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vJoyMapping.LeoBodnar.BBI32.Mapping;

namespace vJoyMapping.LeoBodnar.BBI32
{
    public class Controller : Common.Controller, IDisposable
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<IDisposable> Disposables { get; set; }

        public void Initialize(string devicePath)
        {
            var joystick = new Usb.GameControllers.LeoBodnar.BBI32.Joystick(devicePath);

            MapControls(joystick);

            joystick.Initialize();
        }

        public void MapControls(Usb.GameControllers.LeoBodnar.BBI32.Joystick bbi32)
        {
            // Add in the mappings
            Disposables = new List<IDisposable> {
                bbi32.Subscribe(x => BBI32ButtonStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
                bbi32.Subscribe(x => BBI32UtilCommandsStateHandler.Process(x, this), ex => log.Error($"Exception : {ex}")),
            };
        }

        public void Dispose()
        {
            foreach (var disposable in Disposables)
                disposable?.Dispose();
        }
    }
}
