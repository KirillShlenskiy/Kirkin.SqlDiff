using System.Linq;

namespace Kirkin.Diff.DiffResults
{
    internal sealed class CompositeDiffResult : DiffResult
    {
        internal override string Message
        {
            get
            {
                return string.Empty;
            }
        }

        public CompositeDiffResult(string name, DiffResult[] entries)
            : base(name, entries.Length == 0 || entries.All(e => e.AreSame), entries)
        {
        }
    }
}