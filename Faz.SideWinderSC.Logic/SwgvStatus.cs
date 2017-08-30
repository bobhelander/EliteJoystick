using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Represents the state of a Strategic Commander.
    /// </summary>
    public struct SwgvStatus
    {
        /// <summary>
        /// An empty status.
        /// </summary>
        public static readonly SwgvStatus Empty = new SwgvStatus() { DownButtons = new SwgvButton[0] };

        /// <summary>
        /// Gets of sets the pressed buttons.
        /// </summary>
        public SwgvButton[] DownButtons { get; set; }

        /// <summary>
        /// Gets of sets the pressed buttons.
        /// </summary>
        public byte Buttons { get; set; }

        /// <summary>
        /// Creates a <see cref="SwgvStatus"/> from the output bytes of the controller.
        /// </summary>
        /// <param name="values">The output bytes of the controller.</param>
        /// <returns>The associated status.</returns>
        /// <remarks>
        /// The bytes of the <paramref name="values"/> parameter are in reverse order.
        /// </remarks>
        internal static SwgvStatus Create(byte[] values)
        {
            // https://sourceforge.net/projects/ts3gamevoice/

            //enum Command { NONE = 0, ALL = 1, TEAM = 2, CHANNEL_1 = 4, CHANNEL_2 = 8, CHANNEL_3 = 16, CHANNEL_4 = 32, COMMAND = 64, MUTE = 128 };
            //enum Action { DEACTIVATED = 1024, ACTIVATED = 2048 };

            byte b = values[1];

            //// Retrieve pressed buttons
            ICollection<SwgvButton> buttons = new LinkedList<SwgvButton>();
            foreach (SwgvButton value in Enum.GetValues(typeof(SwgvButton)))
            {
                if ((b & (int)value) == (int)value)
                {
                    buttons.Add(value);
                }
            }

            return new SwgvStatus()
            {
                Buttons = b,
                DownButtons = buttons.ToArray(),
            };
        }

        /// <summary>
        /// Provides a string representation of the current status.
        /// </summary>
        /// <returns>
        /// A string containing all the components of the status.
        /// </returns>
        public override string ToString()
        {
            return "test";
            //return string.Format(
            //    CultureInfo.InvariantCulture,
            //    "[profile:{0}] [x,y,r:{1},{2},{3}] [buttons:{4}]",
            //    this.Profile,
            //    this.X,
            //    this.Y,
            //    this.R,
            //    string.Join(",", this.DownButtons));
        }
    }
}
