using System;
using System.Collections.Generic;
using Unity.Collections;

internal static class NativeArrayExtensions
{
    public static void Fill<T1, T2>(this NativeArray<T1> dest, IReadOnlyCollection<T2> source, Func<T2, T1> selector) where T1 : struct
    {
        if (dest.Length < source.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(source), "source length is bigger than dest length.");
        }

        int idx = 0;
        foreach (var item in source)
        {
            dest[idx] = selector(item);
            ++idx;
        }
    }
}
