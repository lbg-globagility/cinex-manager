using System;

namespace Cinex.WinForm
{
    public static class StringExtensions
    {
        public static bool IsEqualTo(this string input, string comparableText, StringComparison comp = StringComparison.OrdinalIgnoreCase) => string.Compare(input, comparableText, comparisonType: comp) == 0;

        public static bool SimilarTo(this string source, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase) => (source?.IndexOf(toCheck, comp) ?? -1) >= 0;
    }
}
