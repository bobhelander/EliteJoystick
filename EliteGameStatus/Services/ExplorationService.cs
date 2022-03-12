using EliteGameStatus.Exploration;
using EliteJoystick.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EliteGameStatus.Services
{
    public class ExplorationService
    {
        ILogger<ExplorationService> logger;

        public ExplorationService(
            ILogger<ExplorationService> logger)
        {
            this.logger = logger;
            logger.LogInformation("---STARTUP-------------------------------------------------------------------------------------------------------");
        }

        public void OutputValuableSystems(StarSystem starSystem) =>
            EliteActions.OutputValuableSystems(starSystem, logger);
    }
}
