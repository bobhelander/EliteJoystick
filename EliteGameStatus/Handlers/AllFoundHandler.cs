using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EliteGameStatus.Handlers
{
    public static class AllFoundHandler
    {
        public static void Process(IEvent apiEvent, ILogger logger, ILogger inGameLogger)
        {
            try
            {
                logger.LogDebug($"{apiEvent.GetType().ToString()}");

                var allBodiesEvent = apiEvent as FssAllBodiesFoundEvent;

                if (null != allBodiesEvent)
                {
                    logger.LogDebug($"All Found: {allBodiesEvent.SystemName}");

                    Task.Run(async () =>
                    {
                        var system = await EdsmConnector.Connector.GetSystem(allBodiesEvent.SystemName).ConfigureAwait(false);

                        logger.LogDebug($"System Received {system?.name}");

                        Exploration.EliteActions.OutputValuableSystems(system, inGameLogger);

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
