using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Mapping
{
    public class Swff2ClipboardStateHandler : IObserver<States>
    {
        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        static readonly UInt32 button6 = (UInt32)Button.Button6;


        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (Controller.TestButtonPressed(current.Buttons, previous.Buttons, button6))
            {
                if (Dapplo.Windows.Clipboard.ContainsText())
                {
                    Controller.TypeFromClipboard();
                }
            }
        }
    }
}
