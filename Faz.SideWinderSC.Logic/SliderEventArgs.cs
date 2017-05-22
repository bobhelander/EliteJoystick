using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the <see cref="Swff2Controller.Slider"/> event.
    /// </summary>
    public sealed class SliderEventArgs : EventArgs
    {
        /// <summary>
        /// The rotation level.
        /// </summary>
        private readonly int slider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderEventArgs"/> class.
        /// </summary>
        /// <param name="slider">The rotation level.</param>
        internal SliderEventArgs(int slider)
        {
            this.slider = slider;
        }

        /// <summary>
        /// Gets the rotation level.
        /// </summary>
        public int Slider { get { return this.slider; } }
    }
}
