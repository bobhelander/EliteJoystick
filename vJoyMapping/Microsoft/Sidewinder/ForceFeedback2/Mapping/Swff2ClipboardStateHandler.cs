using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2ClipboardStateHandler
    {
        static readonly UInt32 button6 = (UInt32)Button.Button6;

        public static void Process(States value, Controller controller)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (controller.TestButtonPressed(current.Buttons, previous.Buttons, button6))
            {
                controller.TypeFromClipboard();
            }
        }
    }
}
