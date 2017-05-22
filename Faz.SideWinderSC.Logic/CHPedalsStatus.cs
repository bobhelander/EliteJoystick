using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Represents the state of a Strategic Commander.
    /// </summary>
    public struct CHPedalsStatus
    {
        /// <summary>
        /// An empty status.
        /// </summary>
        public static readonly CHPedalsStatus Empty = new CHPedalsStatus() { };

        /// <summary>
        /// Gets or sets the selected status.
        /// </summary>
        //public int Profile { get; set; }

        /// <summary>
        /// Gets or sets the x-axis position of the controller.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-axis position of the controller.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the controller.
        /// </summary>
        public int R { get; set; }

        /// <summary>
        /// Gets of sets the pressed buttons.
        /// </summary>
        //public SwscButton[] DownButtons { get; set; }

        /// <summary>
        /// Creates a <see cref="CHPedalsStatus"/> from the output bytes of the controller.
        /// </summary>
        /// <param name="values">The output bytes of the controller.</param>
        /// <returns>The associated status.</returns>
        /// <remarks>
        /// The bytes of the <paramref name="values"/> parameter are in reverse order.
        /// </remarks>
        internal static CHPedalsStatus Create(byte[] values)
        {
            // values[] :   6  5  4  3  2  1  0
            // reserved : [00 00 00 00 00 00 ff]
            // Left Toe : [00 00 00 00 00 ff 00]
            // Right Toe: [00 00 00 00 ff 00 00]
            // R        : [00 00 00 ff 00 00 00]
            int x = values[1];
            int y = values[2];
            int r = values[3];

            // Return result with a final correction on (x, y, r) to manage negative figures (two-components encoding)
            return new CHPedalsStatus()
            {
                X = x,
                Y = y,
                R = r,
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
            //    //this.Profile,
            //    this.X,
            //    this.Y,
            //    this.R
            //    //string.Join(",", this.DownButtons)
            //    );
        }
    }
}
