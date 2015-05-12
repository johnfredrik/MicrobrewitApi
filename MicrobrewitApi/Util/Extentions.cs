using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Util
{
    public static class Extentions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }
}