using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides data for the <see cref="SwscController.ProfileChanged"/> event.
    /// </summary>
    public sealed class ProfileChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The selected profile.
        /// </summary>
        private readonly int profile;

        /// <summary>
        /// Iniializes a new instance of the <see cref="ProfileChangedEventArgs"/> class.
        /// </summary>
        /// <param name="profile">The selected profile.</param>
        internal ProfileChangedEventArgs(int profile)
        {
            this.profile = profile;
        }

        /// <summary>
        /// Gets the selected profile.
        /// </summary>
        public int Profile
        {
            get { return this.profile; }
        }
    }
}
