﻿using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.LeoBodnar.BBI32.Models;
using vJoyMapping.Common;

namespace vJoyMapping.LeoBodnar.BBI32.Mapping
{
    public static class BBI32ButtonStateHandler
    {
        public static void Process(States value, Controller controller)
        {
            // Buttons 21 - 32 On the Commander controller
            uint bbi32CommanderButtons = value.Current.Buttons << (int)(MappedButtons.BBI32Button1 - 1);

            controller.SetJoystickButtons(bbi32CommanderButtons, vJoyTypes.Commander, MappedButtons.BBI32ButtonMask);

            // Button 13 and 14 are mapped to 31 and 32 on the Stick & Pedals

            // Get rid of all the other buttons
            uint bbi32StickButtonsRaw = value.Current.Buttons >> 12;
            uint bbi32StickButtons = bbi32StickButtonsRaw << (int)(MappedButtons.BBI32Button13 - 1);

            controller.SetJoystickButtons(bbi32StickButtons, vJoyTypes.StickAndPedals, MappedButtons.BBI32ButtonMask2);
        }
    }
}
