using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the <see cref="TMWartHogThrottleController.Move"/> event.
    /// </summary>
    public sealed class MoveAxisEventArgs : EventArgs
    {
        /// <summary>
        /// The x-axis position of the controller.
        /// </summary>
        private readonly ushort z;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveAxisEventArgs"/> class.
        /// </summary>
        /// <param name="z">The y-axis position of the controller.</param>
        internal MoveAxisEventArgs(ushort z)
        {
            this.z = z;
        }

        /// <summary>
        /// Gets the x-axis position of the controller.
        /// </summary>
        public ushort Z { get { return this.z; } }
    }
}
