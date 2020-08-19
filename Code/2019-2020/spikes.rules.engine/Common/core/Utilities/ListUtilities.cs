using System.Collections.Generic;
using System.Linq;

namespace core.Utilities
{
    public static class ListUtilities
    {
        public static decimal SumList(this IEnumerable<decimal> list)
        {
            return list.Sum(x => x);
        }
        public static float SumList(this IEnumerable<float> list)
        {
            return list.Sum(x => x);
        }
        public static long SumList(this IEnumerable<long> list)
        {
            return list.Sum(x => x);
        }
        public static int SumList(this IEnumerable<int> list)
        {
            return list.Sum(x => x);
        }
        public static double SumList(this IEnumerable<double> list)
        {
            return list.Sum(x => x);
        }
    }
}
