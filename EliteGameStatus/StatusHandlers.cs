using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace EliteGameStatus
{
    public static class StatusHandlers
    {
        public static void AddHandlers(Status status, IEdsmConnector edsmConnector, ILogger logger, ILogger inGameLogger)
        {
            status.Subscribe(x => Handlers.JumpHandler.Process(x, edsmConnector, logger, inGameLogger));
        }
    }
}
