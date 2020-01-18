using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EliteGameStatus.Handlers
{
    public static class ScanEventHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(EliteAPI.Events.IEvent apiEvent)
        {
            try
            {
                var scanInfoEvent = apiEvent as EliteAPI.Events.ScanInfo;

                if (null != scanInfoEvent)
                {
                    Exploration.EliteActions.OutputValuableBody(scanInfoEvent);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}
