using BrowserMapping;
using EliteJoystick.Common;
using EliteJoystick.Common.Messages;

namespace KeyboardMapping.Mapping
{
    public static class KeyboardFunctionKeysHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(KeyboardMessage value, Controller controller)
        {
            //{"VirutalKey":112,"ScanCode":59,"Flags":0 F1
            if (value.VirutalKey == 112 && value.Flags == 1)    // F1
            {
                Actions.MaterialTradersEdsm(controller.GameStatus.StarSystem);
                log.Debug($"Material Traders near {controller.GameStatus.StarSystem}");
            }

            if (value.VirutalKey == 113 && value.Flags == 1)    // F2
            {
                Actions.InterstellarFactorsContactEdsm(controller.GameStatus.StarSystem);
                log.Debug($"Interstellar Factors Contact near {controller.GameStatus.StarSystem}");
            }

            if (value.VirutalKey == 114 && value.Flags == 1)    // F3
            {
                Actions.HighGradeEmissionsEdsm(controller.GameStatus.StarSystem);
                log.Debug($"High Grade Emissions near {controller.GameStatus.StarSystem}");
            }

            if (value.VirutalKey == 115 && value.Flags == 1)    // F4
            {
                Actions.BestSellCommodity("350");
                log.Debug($"Best Sell Void Opals {controller.GameStatus.StarSystem}");
            }

            if (value.VirutalKey == 116 && value.Flags == 1)    // F5
            {
                Actions.BestSellCommodity("83");
                log.Debug($"Best Sell Painite {controller.GameStatus.StarSystem}");
            }

            if (value.VirutalKey == 117 && value.Flags == 1)    // F6
            {
                Actions.TechnologyBrokersEdsm(controller.GameStatus.StarSystem);
                log.Debug($"Technology Brokers near {controller.GameStatus.StarSystem}");
            }
        }
    }
}
