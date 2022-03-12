using EliteJoystick.Common;
using EliteJoystick.Common.Logic;
using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Linq;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleSecondaryFireCommand
    {
        //Observable.Interval: To create the heartbeat the the button will be pressed on
        private static readonly IObservable<int> timer = Observable.Interval(TimeSpan.FromMilliseconds(500)).Select(_ => 1);

        private static readonly KeyCode[] R_CTRL_R_ALT = new KeyCode[] { KeyMap.ModifierKeyNameMap["KEY_MOD_RCTRL"], KeyMap.ModifierKeyNameMap["KEY_MOD_RALT"] };
        private static readonly byte KEY_BACKSLASH_CODE = KeyMap.KeyNameMap["KEY_BACKSLASH"].Code;

        // This will be non null while the action is running
        private static IDisposable secondaryFire = null;

        private const UInt32 SpeedbrakeForward = (UInt32)Button.Button07;

        public static void Process(States value, Controller controller)
        {
            // Use the speedbrake to lock the secondary fire button down.  
            // This will hold the mining laser or discovery scanner on until the switch is moved off.
            // In fighting mode this switch is used to cycle the subsystem targeting.

            if (Reactive.ButtonDown(value, SpeedbrakeForward) &&
               (controller.SharedState.CurrentMode == EliteSharedState.Mode.Travel ||
                controller.SharedState.CurrentMode == EliteSharedState.Mode.Mining))
            {
                controller.SharedState.SecondaryFireActive = true;

                if (secondaryFire == null)
                {
                    secondaryFire = timer.Subscribe(async (_) =>
                    {
                        await controller.PressKeyAsync(KEY_BACKSLASH_CODE, R_CTRL_R_ALT, -1).ConfigureAwait(false);

                        controller.Logger.LogDebug("Secondary Fire: Pressed");
                    });
                    controller.Logger.LogDebug("Secondary Fire: Running");
                }

                controller.SetJoystickButton(true, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
            else if (controller.SharedState.SecondaryFireActive)
            {
                controller.SharedState.SecondaryFireActive = false;
                
                if (secondaryFire != null)
                {
                    secondaryFire.Dispose();
                    secondaryFire = null;
                    controller.ReleaseKey(KEY_BACKSLASH_CODE, R_CTRL_R_ALT);

                    controller.Logger.LogDebug("Secondary Fire: Stopped");
                }

                controller.SetJoystickButton(false, MappedButtons.SecondaryFire, vJoyTypes.Virtual);
            }
        }
    }
}
