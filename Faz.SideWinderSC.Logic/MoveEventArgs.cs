using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the X Y Axis move event.
    /// </summary>
    public sealed class MoveEventArgs : EventArgs
    {
        /// <summary>
        /// The x-axis position of the controller.
        /// </summary>
        private readonly int x;

        /// <summary>
        /// The y-axis position of the controller.
        /// </summary>
        private readonly int y;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveEventArgs"/> class.
        /// </summary>
        /// <param name="x">The x-axis position of the controller.</param>
        /// <param name="y">The y-axis position of the controller.</param>
        internal MoveEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets the x-axis position of the controller.
        /// </summary>
        public int X { get { return this.x; } }

        /// <summary>
        /// Gets the y-axis position of the controller.
        /// </summary>
        public int Y { get { return this.y; } }
    }
}
