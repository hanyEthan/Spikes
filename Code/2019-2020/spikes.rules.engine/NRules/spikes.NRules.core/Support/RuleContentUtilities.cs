using System.Collections.Generic;

namespace spikes.NRules.core.Support
{
    public static class RuleContentUtilities
    {
        public static string Parse(this Dictionary<string, object> content, string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (content == null) return null;

            object result = content;
            var paths = path.Split('.');

            foreach (var pathLevel in paths)
            {
                if (result == null)
                {
                    return null;
                }
                else if (result is string)
                {
                    return null;
                }
                else if (result is List<object>)
                {
                    var L = result as List<object>;
                    result = int.TryParse(pathLevel, out int i) ? i >= 0 && i < L.Count ? L[i] : null : null;
                }
                else if (result is Dictionary<string, object>)
                {
                    var D = result as Dictionary<string, object>;
                    result = D.TryGetValue(pathLevel, out object V) ? V : null;
                }
            }

            return result != null && result is string ? result.ToString() : null;
        }
    }
}
