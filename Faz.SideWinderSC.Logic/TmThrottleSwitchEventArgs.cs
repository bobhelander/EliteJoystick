using System;
using System.Collections.Generic;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// </summary>
    public sealed class TmThrottleSwitchEventArgs : EventArgs
    {
        /// <summary>
        /// The associated state.
        /// </summary>
        public UInt32 Buttons { get; set; }
        public UInt32 PreviousButtons { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TmThrottleButtonEventArgs"/> class.
        /// </summary>
        internal TmThrottleSwitchEventArgs(
            UInt32 buttons,
            UInt32 previousButtons)
        {
            Buttons = buttons;
            PreviousButtons = previousButtons;
        }
    }
}
