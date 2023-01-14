using EliteAPI.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace EliteJoystickService.Mappings
{
    public static class GameStatusMapping
    {
        public static void Process(IEvent statusEvent, ILogger logger)
        {
            logger?.LogDebug($"{statusEvent.GetType()}");
        }
    }
}
