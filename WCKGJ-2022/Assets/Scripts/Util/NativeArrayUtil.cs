using System;
using System.Collections.Generic;
using Unity.Collections;

public static class NativeArrayUtil
{
    public static void Fill<T1, T2>(NativeArray<T2> dest, IReadOnlyCollection<T1> source, Func<T1, T2> selector) where T2 : struct
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