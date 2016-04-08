using System;
using System.Text;

namespace Kirkin.Diff.DiffResults
{
    internal static class DiffTextUtil
    {
        public static string ToString(object obj)
        {
            if (obj == null) return "NULL";

            if (obj is Array)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append('[');

                foreach (object item in (Array)obj)
                {
                    if (sb.Length > 1)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(ToString(item));
                }

                sb.Append(']');

                return sb.ToString();
            }

            return obj.ToString();
        }
    }
}