using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the slider event.
    /// </summary>
    public sealed class SliderEventArgs : EventArgs
    {
        /// <summary>
        /// The slider level.
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
        /// Gets the slider level.
        /// </summary>
        public int Slider { get { return this.slider; } }
    }
}
