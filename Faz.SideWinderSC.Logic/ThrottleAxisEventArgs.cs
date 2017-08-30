using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the Move event.
    /// </summary>
    public sealed class ThrottleAxisEventArgs : EventArgs
    {
        /// <summary>
        /// The axis position of the controller.
        /// </summary>
        private readonly ushort axisValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottleAxisEventArgs"/> class.
        /// </summary>
        /// <param name="value">The axis position of the controller.</param>
        internal ThrottleAxisEventArgs(ushort value)
        {
            this.axisValue = value;
        }

        /// <summary>
        /// Gets the axis position of the controller.
        /// </summary>
        public ushort AxisValue { get { return this.axisValue; } }
    }
}
