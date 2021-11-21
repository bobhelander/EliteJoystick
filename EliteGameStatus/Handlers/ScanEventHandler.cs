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
    public static class ScanEventHandler
    {
        public static void Process(IEvent apiEvent, ILogger logger, ILogger inGameLogger)
        {
            try
            {
                var scanInfoEvent = apiEvent as ScanEvent;

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
