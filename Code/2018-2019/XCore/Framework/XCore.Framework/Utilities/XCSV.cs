using System.Collections.Generic;
using System.Linq;

namespace XCore.Framework.Utilities
{
    public static class XCSV
    {
        public static List<string> SplitCSV(this string csv, bool nullOrWhitespaceInputReturnsNull = true)
        {
            if (string.IsNullOrWhiteSpace(csv))
            {
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();
            }

            return csv
                  .TrimEnd(',')
                  .Split(',')
                  .AsEnumerable<string>()
                  .Select(s => s.Trim())
                  .ToList();
        }
    }
}
