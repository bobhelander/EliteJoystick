using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Faz.SideWinderSC.Logic
{
    public class ButtonStateEventArgs : EventArgs
    {
        public UInt32 ButtonsState { get; set; }
        public UInt32 PreviousButtonsState { get; set; }
    }
}
