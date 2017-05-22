using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the <see cref="SwscController.Rotate"/> event.
    /// </summary>
    public sealed class RotateEventArgs : EventArgs
    {
        /// <summary>
        /// The rotation level.
        /// </summary>
        private readonly int r;

        /// <summary>
        /// Initializes a new instance of the <see cref="RotateEventArgs"/> class.
        /// </summary>
        /// <param name="r">The rotation level.</param>
        internal RotateEventArgs(int r)
        {
            this.r = r;
        }

        /// <summary>
        /// Gets the rotation level.
        /// </summary>
        public int R { get { return this.r; } }
    }
}
