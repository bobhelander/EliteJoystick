using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EliteGameStatus.Handlers
{
    public static class JumpHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(EliteAPI.Events.IEvent apiEvent)
        {
            try
            {
                log.Debug($"{apiEvent.GetType().ToString()}");

                var startJumpEvent = apiEvent as EliteAPI.Events.StartJumpInfo;

                if (null != startJumpEvent)
                {
                    log.Debug($"{startJumpEvent.JumpType} Jump Started: {startJumpEvent?.StarSystem}");

                    if (startJumpEvent.JumpType == "Hyperspace" && null != startJumpEvent?.StarSystem)
                    {
                        log.Debug($"{startJumpEvent.JumpType} Jump Started: {startJumpEvent?.StarSystem}: Validated");

                        Task.Run(async () =>
                        {
                            var system = await EdsmConnector.Connector.GetSystem(startJumpEvent.StarSystem).ConfigureAwait(false);

                            log.Debug($"System Received {system?.name}");

                            Exploration.EliteActions.OutputValuableSystems(system);

                        }).ContinueWith(t =>
                        {
                            if (t.IsCanceled) log.Error("JumpStarted Canceled");
                            else if (t.IsFaulted) log.Error($"JumpStarted Exception: {t.Exception}");
                            else log.Debug("JumpStarted Event Complete");
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}
