using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Represents the state of a CH Pedals Controller.
    /// </summary>
    public struct CHPedalsStatus
    {
        /// <summary>
        /// An empty status.
        /// </summary>
        public static readonly CHPedalsStatus Empty = new CHPedalsStatus() { };

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
        /// Creates a <see cref="CHPedalsStatus"/> from the output bytes of the controller.
        /// </summary>
        /// <param name="values">The output bytes of the controller.</param>
        /// <returns>The associated status.</returns>
        /// <remarks>
        /// The bytes of the <paramref name="values"/> parameter are in reverse order.
        /// </remarks>
        internal static CHPedalsStatus Create(byte[] values)
        {
            return new CHPedalsStatus()
            {
                X = (int)values[1],
                Y = (int)values[2],
                R = (int)values[3],
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
