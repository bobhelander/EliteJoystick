using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Mapping
{
    public static class SwCommanderProfileHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (current.Profile == previous.Profile)
                return; // No Change

            log.Debug($"Profile changed to {current.Profile}");

            if (1 == current.Profile)
            {
                log.Debug("Entering Program Mode");
                controller.SharedState.ChangeProgramMode(true);
            }
            else
            {
                if (controller.SharedState.ProgramIdsMode)
                {
                    log.Debug("Exiting Program Mode");
                    controller.SharedState.ChangeProgramMode(false);
                    controller.Settings.Save();  // Commit changes
                }
            }
        }
    }
}
