using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the <see cref="Controller.Read"/> event.
    /// </summary>
    public sealed class ReadEventArgs : EventArgs
    {
        /// <summary>
        /// The output values.
        /// </summary>
        private readonly byte[] values;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadEventArgs"/> class.
        /// </summary>
        /// <param name="values">The output values.</param>
        internal ReadEventArgs(byte[] values)
        {
            this.values = values;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadEventArgs"/> class
        /// from a buffer.
        /// </summary>
        /// <param name="buffer">The buffer containing the output values.</param>
        /// <param name="size">The length of the output in the buffer.</param>
        /// <param name="counter">Sequential event counter.</param>
        /// <remarks>
        /// The values of the buffer are copied to avoid a link to a modifiable buffer.
        /// </remarks>
        internal ReadEventArgs(byte[] buffer, int size, ulong counter)
        {
            this.values = new byte[size];
            Buffer.BlockCopy(buffer, 0, this.values, 0, size);
            Counter = counter;
        }

        /// <summary>
        /// Gets the output values.
        /// </summary>
        public byte[] Values
        {
            get { return this.values; }
        }

        /// <summary>
        /// Sequential event counter
        /// </summary>
        public ulong Counter { get; private set; }
    }
}
