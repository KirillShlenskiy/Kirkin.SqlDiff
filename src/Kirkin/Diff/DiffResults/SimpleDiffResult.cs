using System.Collections;

namespace Kirkin.Diff.DiffResults
{
    internal sealed class SimpleDiffResult : DiffResult
    {
        private readonly object X;
        private readonly object Y;

        internal override string Message
        {
            get
            {
                return AreSame
                    ? string.Empty
                    : $"{DiffTextUtil.ToString(X)} | {DiffTextUtil.ToString(Y)}";
            }
        }

        internal SimpleDiffResult(string name, object x, object y, IEqualityComparer comparer)
            : base(name, comparer.Equals(x, y), EmptyDiffResultArray)
        {
            X = x;
            Y = y;
        }
    }
}