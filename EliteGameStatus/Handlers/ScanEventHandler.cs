using EliteAPI.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System;

namespace EliteGameStatus.Handlers
{
    public static class ScanEventHandler
    {
        public static void Process(IEvent apiEvent, ILogger logger, ILogger inGameLogger)
        {
            try
            {
                if (apiEvent is EliteAPI.Events.ScanEvent)
                {
                    Exploration.EliteActions.OutputValuableBody((EliteAPI.Events.ScanEvent)apiEvent, inGameLogger);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
