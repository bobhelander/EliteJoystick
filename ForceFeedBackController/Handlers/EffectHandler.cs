using EliteAPI.Event.Models.Abstractions;
using EliteAPI.Status.Raw;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ForceFeedBackController.Handlers
{
    public static class EffectHandler
    {
        public static void Process(Controller controller, string key, ILogger logger)
        {
            try
            {
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
                    case "EliteAPI.Event.Models.SupercruiseEntryEvent":
                        controller.PlayFileEffect("Supercruise2.ffe", 750);
                        break;
                    case "EliteAPI.Event.Models.HullDamageEvent":
                        controller.PlayFileEffect("HullDamage.ffe", 250);
                        break;

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
