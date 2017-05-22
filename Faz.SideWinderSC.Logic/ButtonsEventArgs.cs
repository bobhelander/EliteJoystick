using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the <see cref="ButtonsEventArg"/> and
    /// <see cref="ButtonsEventArg"/> events.
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
