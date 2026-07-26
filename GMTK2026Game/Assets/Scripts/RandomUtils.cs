using System;
using System.Collections.Generic;
using System.Linq;
public static class RandomUtils
{
    public static IEnumerable<T> RandomSubset<T>(IEnumerable<T> source, int count, Random rng = null)
    {
        var list = source.ToList();
        if (count > list.Count)
        {
            throw new ArgumentException("count cannot be larger than list size");
        }
        rng ??= new Random();
        for (int i = 0; i < count; i++)
        {
            int j = rng.Next(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list.Take(count).ToList();
    }
    public static List<T> RandomlyReorderList<T>(List<T> list, Random rng = null)
    {
        var remainingElements = list.ToList();
        List<T> result = new();
        rng ??= new Random();
        while (remainingElements.Any())
        {
            int index = rng.Next(remainingElements.Count);
            result.Add(remainingElements[index]);
            remainingElements.RemoveAt(index);
        }
        return result;
    }
}