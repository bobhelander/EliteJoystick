using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Represents the state of a Keypad.
    /// </summary>
    public struct KeypadStatus
    {
        /// <summary>
        /// An empty status.
        /// </summary>
        public static readonly KeypadStatus Empty = new KeypadStatus() { };

        /// <summary>
        /// Gets or sets the buttons of the controller.
        /// </summary>
        public uint Buttons { get; set; }

        /// <summary>
        /// Creates a <see cref="KeypadStatus"/> from the output bytes of the controller.
        /// </summary>
        /// <param name="values">The output bytes of the controller.</param>
        /// <returns>The associated status.</returns>
        /// <remarks>
        /// The bytes of the <paramref name="values"/> parameter are in reverse order.
        /// </remarks>
        internal static KeypadStatus Create(byte[] values)
        {
            uint v1 = values[0];
            uint v2 = values[1];
            uint v3 = values[2];

            //v2 &= 0x38;

            //if (v2 == 35)  // 00100011 html
            //{
            //}
            //if (v2 == 138) // 10001010 Mail
            //{
            //}
            //if (v2 == 146) // 10010010 calculator
            //{
            //}

            return new KeypadStatus()
            {                
                Buttons = v2 & 0x38
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
            return string.Empty;
        }
    }
}
