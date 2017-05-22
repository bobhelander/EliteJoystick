using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the <see cref="Swff2Controller.Hat"/> event.
    /// </summary>
    public sealed class HatEventArgs : EventArgs
    {
        /// <summary>
        /// The rotation level.
        /// </summary>
        private readonly int hat;

        private readonly int hatSwitch;

        /// <summary>
        /// Initializes a new instance of the <see cref="HatEventArgs"/> class.
        /// </summary>
        /// <param name="hat">The rotation level.</param>
        internal HatEventArgs(int hat, int hatSwitch = 0)
        {
            this.hat = hat;
            this.hatSwitch = hatSwitch;
        }

        /// <summary>
        /// Gets the rotation level.
        /// </summary>
        public int Hat { get { return this.hat; } }

        /// <summary>
        /// Gets the rotation level.
        /// </summary>
        public int HatSwitch { get { return this.hatSwitch; } }
    }
}
