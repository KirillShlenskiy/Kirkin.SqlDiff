namespace Kirkin.Diff.DiffResults
{
    internal sealed class CustomDiffResult : DiffResult
    {
        internal override string Message { get; }

        internal CustomDiffResult(string name, bool areSame, string message)
            : base(name, areSame, EmptyDiffResultArray)
        {
            Message = message;
        }
    }
}