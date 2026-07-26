using System.Collections.Generic;

public static class Ext
{
    public static IEnumerable<T> Except<T>(this IEnumerable<T> values, T excludedItem)
    {
        foreach (var val in values)
        {
            if (val.Equals(excludedItem))
            {
                yield return val;
            }
        }
    }
}