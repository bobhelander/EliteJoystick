using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EliteGameStatus.Handlers
{
    public static class ScanEventHandler
    {
        public static void Process(EliteAPI.Events.IEvent apiEvent, ILogger logger, ILogger inGameLogger)
        {
            try
            {
                var scanInfoEvent = apiEvent as EliteAPI.Events.ScanInfo;

                if (null != scanInfoEvent)
                {
                    Exploration.EliteActions.OutputValuableBody(scanInfoEvent, inGameLogger);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
