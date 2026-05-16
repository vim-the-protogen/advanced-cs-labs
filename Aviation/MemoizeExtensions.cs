using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal static class MemoizeExtensions
{
    public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        where T : notnull
        where TResult : notnull
    {
        var cache = new Dictionary<T, TResult>();
        return arg =>
        {
            if (cache.TryGetValue(arg, out TResult result))
            {
                return result;
            }
            return cache[arg] = func(arg);
        };
    }
}
