using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public static class Swff2ClipboardStateHandler
    {
        private const UInt32 button6 = (UInt32)Button.Button6;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, button6))
            {
                Task.Run(async () => await controller.TypeFromClipboard().ConfigureAwait(false));
            }
        }
    }
}
