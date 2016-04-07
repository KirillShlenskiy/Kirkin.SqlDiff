using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Kirkin.Diff
{
    /// <summary>
    /// Represents the result of a diff operation.
    /// </summary>
    public class DiffResult
    {
        /// <summary>
        /// Simle <see cref="DiffResult"/> factory.
        /// </summary>
        public static DiffResult Create(string name, object x, object y, IEqualityComparer comparer = null)
        {
            return new SimpleDiffResult(name, x, y, comparer ?? PrimitiveEqualityComparer.Instance);
        }

        private static string ToString(object obj)
        {
            if (obj == null) return "NULL";

            if (obj is Array)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append('[');

                foreach (object item in (Array)obj)
                {
                    if (sb.Length > 1) {
                        sb.Append(", ");
                    }

                    sb.Append(ToString(item));
                }

                sb.Append(']');

                return sb.ToString();
            }

            return obj.ToString();
        }

        internal static readonly DiffResult[] EmptyDiffResultArray = new DiffResult[0];

        /// <summary>
        /// Child diff entries.
        /// </summary>
        public DiffResult[] Entries { get; }

        public bool AreSame { get; }

        /// <summary>
        /// Name of the comparison.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Diff message or null if the comparands are identical.
        /// </summary>
        internal virtual string Message { get; }

        sealed class SimpleDiffResult : DiffResult
        {
            private readonly object X;
            private readonly object Y;

            internal override string Message
            {
                get
                {
                    return AreSame
                        ? string.Empty
                        : $"{ToString(X)} | {ToString(Y)}";
                }
            }

            internal SimpleDiffResult(string name, object x, object y, IEqualityComparer comparer)
                : base(name, comparer.Equals(x, y))
            {
                X = x;
                Y = y;
            }
        }

        public DiffResult(string name, bool areSame)
        {
            Name = name;
            Entries = EmptyDiffResultArray;
            AreSame = areSame;
        }

        public DiffResult(string name, DiffResult[] entries)
        {
            Name = name;
            Entries = entries;
            AreSame = Entries.Length == 0 || Entries.All(e => e.AreSame);
        }

        public override string ToString()
        {
            return ToString(DiffTextFormat.Flat);
        }

        public string ToString(DiffTextFormat format)
        {
            if (format == DiffTextFormat.Flat) return DiffDescriptionBuilder.BuildFlatDiffMessage(this);
            if (format == DiffTextFormat.Indented) return DiffDescriptionBuilder.BuildIndentedDiffMessage(this);

            throw new NotImplementedException($"Unknown {nameof(DiffTextFormat)} value.");
        }

        static class DiffDescriptionBuilder
        {
            public static string BuildFlatDiffMessage(DiffResult diffResult)
            {
                if (diffResult == null) throw new ArgumentNullException(nameof(diffResult));

                StringBuilder sb = new StringBuilder();

                BuildFlatMessage(sb, string.Empty, diffResult);

                return sb.ToString();
            }

            private static void BuildFlatMessage(StringBuilder sb, string line, DiffResult diffResult)
            {
                if (!diffResult.AreSame)
                {
                    if (line.Length != 0) {
                        line += " -> ";
                    }

                    line += diffResult.Name;

                    if (!string.IsNullOrEmpty(diffResult.Message))
                    {
                        line += ": ";
                        line += diffResult.Message;
                    }

                    if (diffResult.Entries.Length == 0)
                    {
                        if (sb.Length != 0) {
                            sb.AppendLine();
                        }

                        sb.Append(line);
                    }
                    else
                    {
                        foreach (DiffResult childEntry in diffResult.Entries) {
                            BuildFlatMessage(sb, line, childEntry);
                        }
                    }
                }
            }

            public static string BuildIndentedDiffMessage(DiffResult diffResult)
            {
                if (diffResult == null) throw new ArgumentNullException(nameof(diffResult));

                StringBuilder sb = new StringBuilder();

                BuildIndentedMessage(sb, 0, diffResult);

                return sb.ToString();
            }

            private static void BuildIndentedMessage(StringBuilder sb, int indenting, DiffResult diffResult)
            {
                if (!diffResult.AreSame)
                {
                    if (indenting != 0) {
                        sb.Append(new string(' ', indenting * 3));
                    }

                    sb.Append(diffResult.Name);
                    sb.Append(": ");
                    sb.Append(diffResult.Message);
                    sb.AppendLine();

                    foreach (DiffResult childEntry in diffResult.Entries) {
                        BuildIndentedMessage(sb, indenting + 1, childEntry);
                    }
                }
            }
        }
    }
}