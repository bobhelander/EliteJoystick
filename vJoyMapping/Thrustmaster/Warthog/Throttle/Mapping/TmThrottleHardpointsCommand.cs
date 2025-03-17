using EliteJoystick.Common;
using EliteJoystick.Common.Logic;
using Microsoft.Extensions.Logging;
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
        private static readonly KeyCode[] L_CTRL_L_ALT = new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_LCTRL"], KeyMap.ModifierKeyNameMap["KEY_MOD_LALT"] };
        private static readonly byte KEY_H_CODE = KeyMap.KeyNameMap["KEY_H"].Code;

        private const UInt32 EORDown = (UInt32)Button.Button19;

        public static void Process(States value, Controller controller)
        {
            if (controller.SharedState.HardpointsDeployed == false &&
                Reactive.ButtonPressed(value, EORDown))
            {
                // CTRL-ALT-H
                controller.Logger.LogDebug($"Hardpoints out {controller.SharedState.HardpointsDeployed}");

                controller.PressKey(KEY_H_CODE, L_CTRL_L_ALT);

                controller.SharedState.HardpointsDeployed = true;
            }

            if (controller.SharedState.HardpointsDeployed &&
                Reactive.ButtonReleased(value, EORDown))
            {
                // CTRL-ALT-H
                controller.Logger.LogDebug($"Hardpoints in {controller.SharedState.HardpointsDeployed}");

                controller.PressKey(KEY_H_CODE, L_CTRL_L_ALT);

                controller.SharedState.HardpointsDeployed = false;
            }
        }
    }
}
