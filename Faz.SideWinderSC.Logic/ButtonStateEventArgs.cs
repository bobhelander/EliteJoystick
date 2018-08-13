using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonStateEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 ButtonsState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 PreviousButtonsState { get; set; }
    }
}
