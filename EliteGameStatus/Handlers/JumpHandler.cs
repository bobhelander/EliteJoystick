using EliteAPI.Abstractions.Events;
using EliteAPI.Events;
using EliteGameStatus.Services;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EliteGameStatus.Handlers
{
    public static class JumpHandler
    {
        public static void Process(IEvent apiEvent, IEdsmConnector edsmConnector, ExplorationService explorationService, ILogger logger)
        {
            try
            {
                logger.LogDebug($"{apiEvent.GetType()}");                

                if (apiEvent is StartJumpEvent)
                {
                    var startJumpEvent = (StartJumpEvent)apiEvent;

                    logger.LogDebug($"{startJumpEvent.JumpType} Jump Started: {startJumpEvent.StarSystem}");

                    if (startJumpEvent.JumpType == "Hyperspace" && null != startJumpEvent.StarSystem)
                    {
                        logger.LogDebug($"{startJumpEvent.JumpType} Jump Started: {startJumpEvent.StarSystem}: Validated");

                        Task.Run(async () =>
                        {
                            var system = await edsmConnector.GetSystem(startJumpEvent.StarSystem).ConfigureAwait(false);

                            logger.LogDebug($"System Received {system?.name}");

                            explorationService.OutputValuableSystems(system);

                        }).ContinueWith(t =>
                        {
                            if (t.IsCanceled) logger.LogError("JumpStarted Canceled");
                            else if (t.IsFaulted) logger.LogError($"JumpStarted Exception: {t.Exception}");
                            else logger.LogDebug("JumpStarted Event Complete");
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
