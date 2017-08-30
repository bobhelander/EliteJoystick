using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Represents the state of a BBI-32 Controller
    /// </summary>
    public struct BBI32Status
    {
        /// <summary>
        /// An empty status.
        /// </summary>
        public static readonly BBI32Status Empty = new BBI32Status() { };
        
        /// <summary>
        /// Gets or sets the button states controller.
        /// </summary>
        public uint Buttons { get; set; }

        /// <summary>
        /// Creates a <see cref="BBI32Status"/> from the output bytes of the controller.
        /// </summary>
        /// <param name="values">The output bytes of the controller.</param>
        /// <returns>The associated status.</returns>
        /// <remarks>
        /// The bytes of the <paramref name="values"/> parameter are in reverse order.
        /// </remarks>
        internal static BBI32Status Create(byte[] values)
        {            
            uint buttons = BitConverter.ToUInt32(values, 1);

            return new BBI32Status()
            {                
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
            return string.Empty;
        }
    }
}
