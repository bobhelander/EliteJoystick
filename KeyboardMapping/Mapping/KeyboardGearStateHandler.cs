using EliteJoystick.Common.Messages;

namespace KeyboardMapping.Mapping
{
    public static class KeyboardGearStateHandler
    {
        public static void Process(KeyboardMessage value, Controller controller)
        {
            // {Key: 32, ScanCode: 57, Flags: Down}
            if (value.VirutalKey == 32 && value.ScanCode == 57 && value.Flags == 0)
                controller.SharedState.ChangeGear(true);
        }
    }
}
