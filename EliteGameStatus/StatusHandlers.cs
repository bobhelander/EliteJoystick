using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace EliteGameStatus
{
    public static class StatusHandlers
    {
        public static void AddHandlers(Status status, ILogger logger, ILogger inGameLogger)
        {
            status.Subscribe(x => Handlers.JumpHandler.Process(x, logger, inGameLogger));
        }
    }
}
