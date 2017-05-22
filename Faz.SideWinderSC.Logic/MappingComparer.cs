using System;
using System.Collections.Generic;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Compares <see cref="Mapping"/> instances based on their button parameters - ignoring
    /// the associated configuration (name and keys).
    /// </summary>
    internal class MappingComparer : IEqualityComparer<Mapping>
    {
        /// <summary>
        /// The unique instance of the class.
        /// </summary>
        private static MappingComparer instance = new MappingComparer();

        /// <summary>
        /// Gets the unique instance of the class.
        /// </summary>
        public static MappingComparer Instance { get { return instance; } }

        /// <summary>
        /// Prevents new instance of the class from being created.
        /// </summary>
        private MappingComparer()
        { }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
        public bool Equals(Mapping x, Mapping y)
        {
            if (object.ReferenceEquals(x, y))
            {
                // both null or same instance
                return true;
            }

            if (x == null || y == null)
            {
                // one is null
                return false;
            }

            return x.Button == y.Button;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The <paramref name="obj"/> parameter is <c>null</c>.
        /// </exception>
        public int GetHashCode(Mapping obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else
            {
                return (int)obj.Button;
            }
        }
    }
}
