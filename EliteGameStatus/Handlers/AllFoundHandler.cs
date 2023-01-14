using EliteAPI.Abstractions.Events;
using EliteAPI.Events;
using EliteGameStatus.Services;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EliteGameStatus.Handlers
{
    public static class AllFoundHandler
    {
        public static void Process(IEvent apiEvent, IEdsmConnector edsmConnector, ExplorationService explorationService, ILogger logger)
        {
            try
            {
                logger.LogDebug($"{apiEvent.GetType().ToString()}");

                if (apiEvent is FssAllBodiesFoundEvent allBodiesEvent)
                {
                    logger.LogDebug($"All Found: {allBodiesEvent.SystemName}");

                    Task.Run(async () =>
                    {
                        var system = await edsmConnector.GetSystem(allBodiesEvent.SystemName).ConfigureAwait(false);

                        logger.LogDebug($"System Received {system?.name}");

                        explorationService.OutputValuableSystems(system);

                    }).ContinueWith(t =>
                    {
                        if (t.IsCanceled) logger.LogError($"AllBodiesEvent Canceled");
                        else if (t.IsFaulted) logger.LogError($"AllBodiesEvent Exception: {t.Exception}");
                        else logger.LogDebug($"AllBodiesEvent Complete");
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
