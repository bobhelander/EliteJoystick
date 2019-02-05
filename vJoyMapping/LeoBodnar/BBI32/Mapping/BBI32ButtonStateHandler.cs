using EliteJoystick.Common;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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


            //// Buttons 21 - 32 On the Commander controller
            //uint buttonIndex = MappedButtons.BBI32Button1;
            //foreach (UInt32 button in Enum.GetValues(typeof(BBI32Button)))
            //{
            //    if (buttonIndex < 32)
            //    {
            //        controller.SetJoystickButton(
            //            Reactive.ButtonDown(value, button), buttonIndex, vJoyTypes.Commander);
            //    }
            //    else
            //    {
            //        // 33 and 34 are mapped to 31 and 32 on a different controller 
            //        controller.SetJoystickButton(
            //            Reactive.ButtonDown(value, button), buttonIndex-2, vJoyTypes.StickAndPedals);
            //    }

            //    buttonIndex++;
            //}
        }
    }
}
