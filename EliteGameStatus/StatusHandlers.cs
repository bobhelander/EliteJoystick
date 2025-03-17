using EliteGameStatus.Services;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Linq;

namespace EliteGameStatus
{
    public static class StatusHandlers
    {
        public static void AddHandlers(Status status, IEdsmConnector edsmConnector, ExplorationService explorationService, ILogger logger)
        {
            status.Subscribe(x => Handlers.JumpHandler.Process(x, edsmConnector, explorationService, logger));
        }
    }
}
