using EliteJoystick.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Mapping
{
    public static class SwCommanderProgramIds
    {
        public static void Process(States value, Controller controller)
        {
            if (false == controller.SharedState.ProgramIdsMode)
                return; // Not programming

            // Change vJoyTypes.StickAndPedals
            if (Reactive.ButtonPressed(value, (uint)Button.Button1))
            {
                // swap next
                SwapNext(controller, vJoyTypes.StickAndPedals, new string[] { vJoyTypes.Throttle, vJoyTypes.Commander, vJoyTypes.Virtual });
            }

            // Change vJoyTypes.Throttle
            if (Reactive.ButtonPressed(value, (uint)Button.Button2))
            {
                // swap next
                SwapNext(controller, vJoyTypes.Throttle, new string[] { vJoyTypes.Commander, vJoyTypes.Virtual });
            }

            // Change vJoyTypes.Commander
            if (Reactive.ButtonPressed(value, (uint)Button.Button3))
            {
                Swap(controller, vJoyTypes.Commander, vJoyTypes.Virtual);
            }
        }

        private static void SwapLowest(Controller controller, string joystickType, IEnumerable<string> availableTypes)
        {
            // Get items below the current id
            uint vJoyId = controller.Settings.vJoyMapper.GetJoystickId(joystickType);
            SwapLowestId(controller, joystickType, availableTypes.Where(x => controller.Settings.vJoyMapper.GetJoystickId(x) < vJoyId));
        }

        private static void SwapNext(Controller controller, string joystickType, IEnumerable<string> availableTypes)
        {
            // Get items above the current id
            uint vJoyId = controller.Settings.vJoyMapper.GetJoystickId(joystickType);
            var controllersAbove = availableTypes.Where(x => controller.Settings.vJoyMapper.GetJoystickId(x) > vJoyId);
            if (controllersAbove.Any())
                SwapLowestId(controller, joystickType, controllersAbove);
            else
                SwapLowest(controller, joystickType, availableTypes);
        }

        private static void SwapLowestId(Controller controller, string joystickType, IEnumerable<string> availableTypes)
        {
            // Create a temporary map
            var availableMap = availableTypes.ToDictionary(key => key, value => controller.Settings.vJoyMapper.vJoyMap[value]);
            // Get the lowest Id
            var lowestValue = availableMap.OrderBy(kvp => kvp.Value).First();
            Swap(controller, joystickType, lowestValue.Key);
        }

        private static void Swap(Controller controller, string joystickType, string withjoystickType)
        {
            controller.Logger.LogDebug($"Swaping {joystickType}:{controller.Settings.vJoyMapper.vJoyMap[joystickType]} with {withjoystickType}:{controller.Settings.vJoyMapper.vJoyMap[withjoystickType]}");
            uint tempId = controller.Settings.vJoyMapper.vJoyMap[joystickType];
            controller.Settings.vJoyMapper.vJoyMap[joystickType] = controller.Settings.vJoyMapper.vJoyMap[withjoystickType];
            controller.Settings.vJoyMapper.vJoyMap[withjoystickType] = tempId;
        }
    }
}
