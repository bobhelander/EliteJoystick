﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common
{
    /// <summary>
    /// Defined output buttons for the vJoyType.Virtual controller
    /// This defines what button number outputs are reserved
    /// </summary>
    public class MappedButtons
    {
        // Virtual Joystick (Virtual)
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

        // Combined Joystick (StickAndPedals)
        public static uint ForceFeedback2Trigger = 1;
        public static uint ForceFeedback2Button2 = 2;
        public static uint ForceFeedback2Button3 = 3;
        public static uint ForceFeedback2Button4 = 4;
        public static uint ForceFeedback2Button5 = 5;
        public static uint ForceFeedback2Button6 = 6;
        public static uint ForceFeedback2Button7 = 7;
        public static uint ForceFeedback2Button8 = 8;

        public static uint ForceFeedback2HatUp = 9;
        public static uint ForceFeedback2HatRight = 10;
        public static uint ForceFeedback2HatDown = 11;
        public static uint ForceFeedback2HatLeft = 12;
        public static uint ForceFeedback2HatCentered = 13;

        public static uint ThrottleHatUp = 14;
        public static uint ThrottleHatRight = 15;
        public static uint ThrottleHatDown = 16;
        public static uint ThrottleHatLeft = 17;

        // Throttle Neutral
        public static uint ThrottleMSNone = 18;
        public static uint ThrottleSPDM = 19;
        public static uint ThrottleBSM = 20;
        public static uint ThrottleCHM = 21;
        public static uint ThrottlePSM = 22;
        public static uint ThrottleEOLNORM = 23;
        public static uint ThrottleEORNORM = 24;
        public static uint ThrottleFLAPM = 25;
        public static uint ThrottleAPAH = 26;
        // 27 - 32 Unassigned

        // Sidwinder Commander / Game Voice  (Commander)
        public static uint CommanderButton1 = 1;
        public static uint CommanderButton2 = 2;
        public static uint CommanderButton3 = 3;
        public static uint CommanderButton4 = 4;
        public static uint CommanderButton5 = 5;
        public static uint CommanderButton6 = 6;
        public static uint CommanderZoomIn = 7;
        public static uint CommanderZoomOut = 8;
        public static uint CommanderShift1 = 9;
        public static uint CommanderShift2 = 10;
        public static uint CommanderShift3 = 11;
        public static uint CommanderRecord = 12;

        public static uint VoiceButtonAll = 13;
        public static uint VoiceButtonTeam = 14;
        public static uint VoiceButton1 = 15;
        public static uint VoiceButton2 = 16;
        public static uint VoiceButton3 = 17;
        public static uint VoiceButton4 = 18;
        public static uint VoiceCommandButton3 = 19;
        public static uint VoiceMuteButton3 = 20;

        public static uint BBI32Button1 = 21;
        public static uint BBI32Button2 = 22;
        public static uint BBI32Button3 = 23;
        public static uint BBI32Button4 = 24;
        public static uint BBI32Button5 = 25;
        public static uint BBI32Button6 = 26;
        public static uint BBI32Button7 = 27;
        public static uint BBI32Button8 = 28;
        public static uint BBI32Button9 = 29;
        public static uint BBI32Button10 = 30;
        public static uint BBI32Button11 = 31;
        public static uint BBI32Button12 = 32;
    }
}
