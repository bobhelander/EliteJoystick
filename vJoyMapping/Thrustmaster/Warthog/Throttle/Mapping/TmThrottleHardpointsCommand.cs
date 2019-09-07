using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleHardpointsCommand
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const UInt32 EORDown = (UInt32)Button.Button19;

        public static void Process(States value, Controller controller)
        {
            if (controller.SharedState.HardpointsDeployed == false &&
                Reactive.ButtonPressed(value, EORDown))
            {
                log.Debug($"Hardpoints out {controller.SharedState.HardpointsDeployed}");
                Task.Run(async () => await controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x48).ConfigureAwait(false));
                controller.SharedState.HardpointsDeployed = true;
            }

            if (controller.SharedState.HardpointsDeployed == true &&
                Reactive.ButtonReleased(value, EORDown))
            {
                log.Debug($"Hardpoints in {controller.SharedState.HardpointsDeployed}");
                Task.Run(async () => await controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x48).ConfigureAwait(false));
                controller.SharedState.HardpointsDeployed = false;
            }
        }
    }
}
