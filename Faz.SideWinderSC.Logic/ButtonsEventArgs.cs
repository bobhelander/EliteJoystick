using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ButtonsEventArgs : EventArgs
    {
        /// <summary>
        /// The associated button.
        /// </summary>
        public uint Buttons { get; set; }

        /// <summary>
        /// The associated button.
        /// </summary>
        public uint PreviousButtons { get; set; }
    }
}
