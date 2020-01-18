using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace EliteGameStatus
{
    public static class StatusHandlers
    {
        public static void AddHandlers(Status status)
        {
            status.Subscribe(x => Handlers.JumpHandler.Process(x));
        }
    }
}
