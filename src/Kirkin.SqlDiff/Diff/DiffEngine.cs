using System.Collections;

namespace Kirkin.Diff
{
    /// <summary>
    /// Base class for all diff engines.
    /// </summary>
    public abstract class DiffEngine<T>
    {
        /// <summary>
        /// When overridden in a derived class, performs a deep comparison
        /// of the given objects using the given comparer and returns the results
        /// wrapped in a single <see cref="DiffResult"/> aggregate with the given name.
        /// </summary>
        protected internal abstract DiffResult Compare(string name, T x, T y, IEqualityComparer comparer);
    }
}