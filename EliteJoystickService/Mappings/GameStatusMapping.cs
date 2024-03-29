﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickService.Mappings
{
    public static class GameStatusMapping
    {
        public static void Process(EliteAPI.Events.IEvent statusEvent, ILogger logger)
        {
            logger?.LogDebug($"{statusEvent.GetType()}");
        }
    }
}
