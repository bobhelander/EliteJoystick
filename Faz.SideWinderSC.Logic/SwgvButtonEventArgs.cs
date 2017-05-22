using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the <see cref="SwgvController.ButtonUp"/> and
    /// <see cref="SwscController.ButtonDown"/> events.
    /// </summary>
    public sealed class SwgvButtonEventArgs : EventArgs
    {
        /// <summary>
        /// The associated button.
        /// </summary>
        private readonly SwgvButton button;

        /// <summary>
        /// A value indicating whether the button is down.
        /// </summary>
        private readonly bool down;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwgvButtonEventArgs"/> class.
        /// </summary>
        /// <param name="button">The associated button.</param>
        /// <param name="down">A value indicating whether the button is down.</param>
        internal SwgvButtonEventArgs(SwgvButton button, bool down)
        {
            this.button = button;
            this.down = down;
        }

        /// <summary>
        /// Gets the associated button.
        /// </summary>
        public SwgvButton Button
        {
            get { return this.button; }
        }

        /// <summary>
        /// Gets a value indicating whether the button is down.
        /// </summary>
        public bool IsDown
        {
            get { return this.down; }
        }

        /// <summary>
        /// Gets a value indicating whether the button is up.
        /// </summary>
        public bool IsUp
        {
            get { return !this.down; }
        }
    }
}
