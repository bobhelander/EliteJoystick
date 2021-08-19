using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Mapping
{
    public static class SwCommanderProfileHandler
    {
        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (current.Profile == previous.Profile)
                return; // No Change

            controller.Logger.LogDebug($"Profile changed to {current.Profile}");

            if (1 == current.Profile)
            {
                controller.Logger.LogDebug("Entering Program Mode");
                controller.SharedState.ChangeProgramMode(true);
            }
            else
            {
                if (controller.SharedState.ProgramIdsMode)
                {
                    controller.Logger.LogDebug("Exiting Program Mode");
                    controller.SharedState.ChangeProgramMode(false);
                    controller.Settings.Save();  // Commit changes
                }
            }
        }
    }
}
