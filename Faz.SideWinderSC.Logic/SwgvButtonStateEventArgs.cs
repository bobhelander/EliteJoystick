using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides button state values
    /// </summary>
    public class SwgvButtonStateEventArgs : EventArgs
    {
        /// <summary>
        /// Current State
        /// </summary>
        public byte ButtonsState { get; set; }

        /// <summary>
        /// Last State
        /// </summary>
        public byte PreviousButtonsState { get; set; }
    }
}
