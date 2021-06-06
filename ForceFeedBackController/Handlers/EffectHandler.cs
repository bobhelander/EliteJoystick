using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ForceFeedBackController.Handlers
{
    public static class EffectHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(Controller controller, EliteAPI.Events.IEvent apiEvent)
        {
            try
            {
                if (apiEvent is EliteAPI.Events.StatusEvent)
                {
                    var statusEvent = apiEvent as EliteAPI.Events.StatusEvent;
                    var key = $"{statusEvent.Event}:{statusEvent.Value}";

                    switch (key)
                    {
                        case "Status.Gear:True":
                        case "Status.Gear:False":
                            controller.PlayFileEffect("gear.ffe", 2000);
                            break;
                        case "Status.Lights:True":
                        case "Status.Lights:False":
                            controller.PlayFileEffect("Vibrate.ffe", 750);
                            break;
                        case "Status.Hardpoints:True":
                        case "Status.Hardpoints:False":
                            controller.PlayFileEffect("Hardpoints.ffe", 1500);
                            break;
                        case "Status.CargoScoop:True":
                        case "Status.CargoScoop:False":
                            controller.PlayFileEffect("Cargo.ffe", 2000);
                            break;
                        case "Status.Landed:True":
                            controller.PlayFileEffect("Landed.ffe", 1000);
                            break;
                        case "Status.Docked:True":
                            controller.PlayFileEffect("Dock.ffe", 1000);
                            break;
                        case "Status.Overheating:True":
                            controller.PlayFileEffect("Vibrate.ffe", 750);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}
