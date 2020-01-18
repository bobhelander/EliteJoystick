using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickService.Mappings
{
    public static class GameStatusMapping
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(EliteAPI.Events.IEvent statusEvent)
        {
            log.Debug($"{statusEvent.GetType().ToString()}");
        }
    }
}
