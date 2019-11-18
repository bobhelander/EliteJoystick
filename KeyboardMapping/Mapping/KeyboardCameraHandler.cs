using EliteJoystick.Common;
using EliteJoystick.Common.Messages;

namespace KeyboardMapping.Mapping
{
    public static class KeyboardCameraHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(KeyboardMessage value, Controller controller)
        {
            // {Key: 32, ScanCode: 57, Flags: Down}
            if (value.VirutalKey == 32 && value.ScanCode == 57 && value.Flags == 0)
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CameraEnabled, 150);
                log.Debug("Camera Enabled");
            }
        }
    }
}
