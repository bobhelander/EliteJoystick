using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick
{
    /// <summary>
    /// Defined output buttons for the vJoyType.Virtual controller
    /// This defines what button number outputs are reserved
    /// </summary>
    public class MappedButtons
    {
        // Virtual Joystick
        // 1-9 Throttle Neutral Buttons  Faz.SideWinderSC.Logic.TmThrottleSwitchNeutral

        public static uint CycleSubsystem = 10;
        public static uint SecondaryFire = 11;
        public static uint TextMessageEntry = 12;
        public static uint HardpointsToggle = 13;
        public static uint LandingGearToggle = 14;
        public static uint Throttle75 = 15;
        public static uint CameraEnabled = 16;
        public static uint LightsToggle = 17;
        public static uint CameraDisabled = 18;
        public static uint OrbitLinesToggle = 19;
        public static uint FreeCameraToggle = 20;
        public static uint CameraAdvanceModeToggle = 21;
        public static uint SilentRunningToggle = 22;


        public static uint ThrottleHatUp = 29;
        public static uint ThrottleHatRight = 30;
        public static uint ThrottleHatDown = 31;
        public static uint ThrottleHatLeft = 32;
    }
}
