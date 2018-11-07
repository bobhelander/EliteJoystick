using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2UtilCommandsStateHandler
    {
        static UInt32 Button8 = (UInt32)Button.Button8;
        static UInt32 Button7 = (UInt32)Button.Button7;
        static UInt32 Button6 = (UInt32)Button.Button6;
        static UInt32 Button5 = (UInt32)Button.Button5;
        static UInt32 Button2 = (UInt32)Button.Button2;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (controller.TestButtonPressed(previous.Buttons, current.Buttons, Button8))
            {
                // Oculus ASW off  CRTL+KP1
                controller.SendKeyCombo(new byte[] { 0x80 }, 0x31);
            }
            if (controller.TestButtonPressed(previous.Buttons, current.Buttons, Button7))
            {
                // Take Picture  ALT-F10
                controller.SendKeyCombo(new byte[] { 0x82 }, 0xCB);
            }

            if (controller.TestButtonPressed(previous.Buttons, current.Buttons, Button5))
            {
                // HUD off  CRTL+ALT+G
                controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x47);
            }

            //if (Controller.TestButtonPressed(previous.Buttons, current.Buttons, Button2) &&
            //    Controller.SharedState.ThrottleShiftStateValue == EliteSharedState.ThrottleShiftState.Shift1)
            //{
                // Focus window
            //    System.Console.WriteLine(Utils.FocusWindow("EliteDangerous64"));
            //}
        }
    }
}
