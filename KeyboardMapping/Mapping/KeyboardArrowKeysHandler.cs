using BrowserMapping;
using EliteJoystick.Common;
using EliteJoystick.Common.Messages;

namespace KeyboardMapping.Mapping
{
    public static class KeyboardArrowKeysHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(KeyboardMessage value, Controller controller)
        {
            if ((value.VirutalKey == (int)KeyCodes.Keys.Up && value.Flags == 1) ||
                (value.VirutalKey == (int)KeyCodes.Keys.W && value.Flags == 1))
            {
                BrowserMapping.Controller.Scroll(-10);
                log.Debug($"Scroll Up");
            }

            if ((value.VirutalKey == (int)KeyCodes.Keys.Down && value.Flags == 1) ||
                (value.VirutalKey == (int)KeyCodes.Keys.S && value.Flags == 1))
            {
                BrowserMapping.Controller.Scroll(10);
                log.Debug($"Scroll Down");
            }

            if ((value.VirutalKey == (int)KeyCodes.Keys.Left && value.Flags == 1) ||
                (value.VirutalKey == (int)KeyCodes.Keys.A && value.Flags == 1))
            {
                BrowserMapping.Controller.ChangeTab(-1);
                log.Debug($"Previous Tab");
            }

            if ((value.VirutalKey == (int)KeyCodes.Keys.Right && value.Flags == 1) ||
                (value.VirutalKey == (int)KeyCodes.Keys.D && value.Flags == 1))
            {
                BrowserMapping.Controller.ChangeTab(1);
                log.Debug($"Next Tab");
            }
        }
    }
}
