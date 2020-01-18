using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EliteGameStatus.Handlers
{
    public static class AllFoundHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(EliteAPI.Events.IEvent apiEvent)
        {
            try
            {
                log.Debug($"{apiEvent.GetType().ToString()}");

                var allBodiesEvent = apiEvent as EliteAPI.Events.FSSAllBodiesFoundInfo;

                if (null != allBodiesEvent)
                {
                    log.Debug($"All Found: {allBodiesEvent.SystemName}");

                    Task.Run(async () =>
                    {
                        var system = await EdsmConnector.Connector.GetSystem(allBodiesEvent.SystemName).ConfigureAwait(false);

                        log.Debug($"System Received {system?.name}");

                        Exploration.EliteActions.OutputValuableSystems(system);

                    }).ContinueWith(t =>
                    {
                        if (t.IsCanceled) log.Error($"AllBodiesEvent Canceled");
                        else if (t.IsFaulted) log.Error($"AllBodiesEvent Exception: {t.Exception}");
                        else log.Debug($"AllBodiesEvent Complete");
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}
