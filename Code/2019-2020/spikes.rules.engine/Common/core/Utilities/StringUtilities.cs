using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace core.Utilities
{
    public class StringUtilities
    {
        public static Stream ToStream(string from)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(from ?? ""));
        }
        public static bool MatchPattern(string data, string pattern)
        {
            if (data == null || pattern == null) return false;

            Regex reg = new Regex(pattern);
            Match match = reg.Match(data.Trim());

            return match.Success && match.Value == data;
        }
        public static bool MatchPattern(object data, object pattern)
        {
            if (!(data is string)) return false;
            if (!(pattern is string)) return false;

            return MatchPattern(data.ToString(), pattern.ToString());
        }
    }
}
