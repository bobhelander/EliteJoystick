using BrowserMapping;
using EliteJoystick.Common;
using EliteJoystick.Common.Messages;

namespace KeyboardMapping.Mapping
{
    public static class KeyboardNumberKeysHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(KeyboardMessage value, Controller controller)
        {
            // Copy a system name to the clipboard

            // {"VirutalKey":49,"ScanCode":2,"Flags":0}  1
            if (value.VirutalKey == 49 && value.Flags == 1)
            {
                var systemName = Actions.GetEdsmSystemRow(1);
                TextCopy.Clipboard.SetText(systemName);
                log.Debug($"Selected system: {systemName}");
            }

            if (value.VirutalKey == 50 && value.Flags == 1)
            {
                var systemName = Actions.GetEdsmSystemRow(2);
                TextCopy.Clipboard.SetText(systemName);
                log.Debug($"Selected system: {systemName}");
            }

            if (value.VirutalKey == 51 && value.Flags == 1)
            {
                var systemName = Actions.GetEdsmSystemRow(3);
                TextCopy.Clipboard.SetText(systemName);
                log.Debug($"Selected system: {systemName}");
            }

            if (value.VirutalKey == 52 && value.Flags == 1)
            {
                var systemName = Actions.GetEdsmSystemRow(4);
                TextCopy.Clipboard.SetText(systemName);
                log.Debug($"Selected system: {systemName}");
            }

            if (value.VirutalKey == 53 && value.Flags == 1)
            {
                var systemName = Actions.GetEdsmSystemRow(5);
                TextCopy.Clipboard.SetText(systemName);
                log.Debug($"Selected system: {systemName}");
            }

            if (value.VirutalKey == 54 && value.Flags == 1)
            {
                var systemName = Actions.GetEdsmSystemRow(6);
                TextCopy.Clipboard.SetText(systemName);
                log.Debug($"Selected system: {systemName}");
            }
        }
    }
}
