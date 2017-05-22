using System;
using System.Globalization;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides extensions methods for the <see cref="SwscButton"/> enumeration.
    /// </summary>
    public static class SwscButtonExtensions
    {
        /// <summary>
        /// Converts a button to its associated light - if any.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="flash">
        /// A value indicating whether the light if expected to flash.
        /// </param>
        /// <returns>
        /// The light code associated the <paramref name="button"/> - or <c>null</c> if no
        /// light is associated to this button (i.e. shift &amp; zoom buttons).
        /// </returns>
        public static SwscLight? ToLight(this SwscButton button, bool flash)
        {
            // Retrieve the name of the button
            string name = Enum.GetName(typeof(SwscButton), button);

            // Try to retrieve a SwscLight with the same name
            SwscLight light;
            if (Enum.TryParse(name, out light))
            {
                if (flash)
                {
                    return (SwscLight)(((int)light) * 2);
                }
                else
                {
                    return light;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
