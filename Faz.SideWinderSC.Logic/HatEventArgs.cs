using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the hat event.
    /// </summary>
    public sealed class HatEventArgs : EventArgs
    {
        /// <summary>
        /// The rotation level.
        /// </summary>
        private readonly int hat;

        /// <summary>
        /// Hat switch value
        /// </summary>
        private readonly int hatSwitch;

        /// <summary>
        /// Initializes a new instance of the <see cref="HatEventArgs"/> class.
        /// </summary>
        /// <param name="hat">The hat position.</param>
        /// <param name="hatSwitch"></param>
        internal HatEventArgs(int hat, int hatSwitch = 0)
        {
            this.hat = hat;
            this.hatSwitch = hatSwitch;
        }

        /// <summary>
        /// Gets the hat position.
        /// </summary>
        public int Hat { get { return this.hat; } }

        /// <summary>
        /// Gets the hat switch value.
        /// </summary>
        public int HatSwitch { get { return this.hatSwitch; } }
    }
}
