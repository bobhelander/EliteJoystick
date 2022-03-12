﻿using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;
using EliteGameStatus.Services;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

                var startJumpEvent = apiEvent as StartJumpEvent;

                if (null != startJumpEvent)
                {
                    logger.LogDebug($"{startJumpEvent.JumpType} Jump Started: {startJumpEvent?.StarSystem}");

                    if (startJumpEvent.JumpType == "Hyperspace" && null != startJumpEvent?.StarSystem)
                    {
                        logger.LogDebug($"{startJumpEvent.JumpType} Jump Started: {startJumpEvent?.StarSystem}: Validated");

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
