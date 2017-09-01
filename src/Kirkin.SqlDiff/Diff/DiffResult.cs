using System;

namespace Kirkin.Diff
{
    /// <summary>
    /// Represents the result of a diff operation.
    /// </summary>
    public abstract class DiffResult
    {
        internal static readonly DiffResult[] EmptyDiffResultArray = new DiffResult[0];

        /// <summary>
        /// Child diff entries.
        /// </summary>
        public DiffResult[] Entries { get; }

        /// <summary>
        /// True if no differences are detected.
        /// </summary>
        public bool AreSame { get; }

        /// <summary>
        /// Name of the comparison.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Diff message or null if the comparands are identical.
        /// </summary>
        internal abstract string Message { get; }

        protected DiffResult(string name, bool areSame, DiffResult[] entries)
        {
            Name = name;
            AreSame = areSame;
            Entries = entries;
        }

        /// <summary>
        /// Returns a textual description of this diff.
        /// </summary>
        public override string ToString()
        {
            return ToString(DiffTextFormat.List);
        }

        /// <summary>
        /// Returns a textual description of this diff.
        /// </summary>
        public string ToString(DiffTextFormat format)
        {
            if (format == DiffTextFormat.List) return DiffTextUtil.BuildListDiffMessage(this);
            if (format == DiffTextFormat.Tree) return DiffTextUtil.BuildTreeDiffMessage(this);

            throw new NotImplementedException($"Unknown {nameof(DiffTextFormat)} value.");
        }
    }
}