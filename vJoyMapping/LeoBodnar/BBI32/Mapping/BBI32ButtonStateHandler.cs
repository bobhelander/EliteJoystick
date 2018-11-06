using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.LeoBodnar.BBI32.Models;
using vJoyMapping.Common;

namespace vJoyMapping.LeoBodnar.BBI32.Mapping
{
    public class BBI32ButtonStateHandler : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(States value)
        {
            var current = value.Current as State;

            // Buttons 21 - 32 On the combined controller
            uint buttonIndex = 21;
            foreach (UInt32 button in Enum.GetValues(typeof(BBI32Button)))
            {
                bool pressed = Controller.TestButtonDown(current.Buttons, button);
                Controller.SetJoystickButton(pressed, buttonIndex, vJoyTypes.StickAndPedals);
                buttonIndex++;
            }
        }
    }
}
