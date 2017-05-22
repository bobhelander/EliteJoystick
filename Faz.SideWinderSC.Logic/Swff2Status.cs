using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Represents the state of a Strategic Commander.
    /// </summary>
    public struct Swff2Status
    {
        /// <summary>
        /// An empty status.
        /// </summary>
        public static readonly Swff2Status Empty = new Swff2Status() { };

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
        /// Gets or sets the rotation of the controller.
        /// </summary>
        public int Slider { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the controller.
        /// </summary>
        public int Hat { get; set; }
        
        /// <summary>
        /// Gets or sets the rotation of the controller.
        /// </summary>
        public uint Buttons { get; set; }

        /// <summary>
        /// Gets of sets the pressed buttons.
        /// </summary>
        //public Swff2Button[] DownButtons { get; set; }

        /// <summary>
        /// Creates a <see cref="Swff2Status"/> from the output bytes of the controller.
        /// </summary>
        /// <param name="values">The output bytes of the controller.</param>
        /// <returns>The associated status.</returns>
        /// <remarks>
        /// The bytes of the <paramref name="values"/> parameter are in reverse order.
        /// </remarks>
        internal static Swff2Status Create(byte[] values)
        {            
            short x = BitConverter.ToInt16(values, 1);
            short y = BitConverter.ToInt16(values, 3);

            int rotation = values[5];
            int slider = values[6];
            int hat = values[7];
            uint buttons = values[8];

            // Return result with a final correction on (x, y, r) to manage negative figures (two-components encoding)
            return new Swff2Status()
            {                
                X = x,
                Y = y,
                R = rotation,
                Slider = slider,
                Hat = hat,
                //DownButtons = buttons.ToArray(),
                Buttons = buttons,
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
            return string.Format(
                CultureInfo.InvariantCulture,
                "[x,y,r:{0},{1},{2}]",
                this.X,
                this.Y,
                this.R);
        }
    }
}
