using System;
using System.Collections;

namespace Kirkin.Diff
{
    /// <summary>
    /// Default equality comparer used by diff engines.
    /// </summary>
    internal sealed class PrimitiveEqualityComparer
        : IEqualityComparer
    {
        /// <summary>
        /// Singleton <see cref="PrimitiveEqualityComparer"/> instance.
        /// </summary>
        public static readonly PrimitiveEqualityComparer Instance = new PrimitiveEqualityComparer();

        private PrimitiveEqualityComparer()
        {
        }

        /// <summary>
        /// Compares two objects for equality.
        /// </summary>
        public new bool Equals(object x, object y)
        {
            IStructuralEquatable strEqX = x as IStructuralEquatable;

            if (strEqX != null)
            {
                IStructuralEquatable strEqY = y as IStructuralEquatable;

                if (strEqY != null)
                {
                    // Most likely array comparison.
                    return strEqX.Equals(strEqY, Instance);
                }
            }

            return object.Equals(x, y);
        }

        /// <summary>
        /// Suppressed.
        /// </summary>
        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}