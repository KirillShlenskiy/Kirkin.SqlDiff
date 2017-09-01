using System;
using System.Text;

namespace Kirkin.Diff
{
    public static class DiffTextUtil
    {
        /// <summary>
        /// Returns the common string representation of the given primitive object.
        /// </summary>
        public static string ToString(object obj)
        {
            if (obj == null || obj == DBNull.Value) return "NULL";

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

        internal static string BuildListDiffMessage(DiffResult diffResult)
        {
            if (diffResult == null) throw new ArgumentNullException(nameof(diffResult));

            StringBuilder sb = new StringBuilder();

            BuildListMessage(sb, string.Empty, diffResult);

            return sb.ToString();
        }

        private static void BuildListMessage(StringBuilder sb, string line, DiffResult diffResult)
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
                        BuildListMessage(sb, line, childEntry);
                    }
                }
            }
        }

        internal static string BuildTreeDiffMessage(DiffResult diffResult)
        {
            if (diffResult == null) throw new ArgumentNullException(nameof(diffResult));

            StringBuilder sb = new StringBuilder();

            BuildTreeMessage(sb, 0, diffResult);

            return sb.ToString();
        }

        private static void BuildTreeMessage(StringBuilder sb, int indenting, DiffResult diffResult)
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
                    BuildTreeMessage(sb, indenting + 1, childEntry);
                }
            }
        }
    }
}