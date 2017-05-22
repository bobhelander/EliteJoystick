using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Faz.SideWinderSC.Logic
{
    public class SwgvButtonStateEventArgs : EventArgs
    {
        public byte ButtonsState { get; set; }
        public byte PreviousButtonsState { get; set; }
    }
}
