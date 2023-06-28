using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class IListExtension
{
    public static T GetRandom<T>(this IList<T> source)
    {
        return source[UnityEngine.Random.Range(0, source.Count)];
    }

    public static void Shuffle<T>(this IList<T> source)
    {
        for (var i = 0; i < source.Count - 1; ++i)
        {
            var r = UnityEngine.Random.Range(i, source.Count);
            (source[r], source[i]) = (source[i], source[r]);
        }
    }

    public static List<List<T>> Partition<T>(this IList<T> source, int noParts)
    {
        return source
            .Select((s, i) => new { s, i })
            .GroupBy(x => x.i % noParts)
            .Select(g => g.Select(x => x.s).ToList())
            .Reverse()
            .ToList();
    }
}
