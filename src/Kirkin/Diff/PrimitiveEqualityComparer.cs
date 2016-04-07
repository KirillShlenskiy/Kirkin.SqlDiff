using System;
using System.Collections;
using System.Collections.Generic;

namespace Kirkin.Diff
{
    internal sealed class PrimitiveEqualityComparer
        : IEqualityComparer<object>, IEqualityComparer
    {
        public static readonly PrimitiveEqualityComparer Instance = new PrimitiveEqualityComparer();

        private PrimitiveEqualityComparer()
        {
        }

        public new bool Equals(object x, object y)
        {
            IStructuralEquatable strEqX = x as IStructuralEquatable;

            if (strEqX != null)
            {
                IStructuralEquatable strEqY = y as IStructuralEquatable;

                if (strEqY != null) {
                    // Most likely array comparison.
                    return strEqX.Equals(strEqY, Instance);
                }
            }

            return object.Equals(x, y);
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}