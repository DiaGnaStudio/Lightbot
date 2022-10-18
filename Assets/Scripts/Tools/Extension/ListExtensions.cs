using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{
    /// <summary>
    /// Shuffle the list in place using the Fisher-Yates method.
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        for (var i = list.Count - 1; i > 1; i--)
        {
            var j = Random.Range(0, i + 1);
            var value = list[j];
            list[j] = list[i];
            list[i] = value;
        }
    }

    /// <summary>
    /// Return a random item from the list.
    /// Sampling with replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Return a random item from the IEnumerable.
    /// Sampling with replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomItem<T>(this IEnumerable<T> list)
    {
        if (list.Count() == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
        return list.ElementAt(UnityEngine.Random.Range(0, list.Count()));
    }

    /// <summary>
    /// Removes a random item from the list, returning that item.
    /// Sampling without replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RemoveRandom<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
        int index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }

    public static List<string> ToString<T>(this IList<T> list)
    {
        List<string> result = new List<string>();

        foreach (var item in list)
        {
            result.Add(item.ToString());
        }

        return result;
    }

    public static string[] ToString<T>(this T[] list)
    {
        var result = new string[list.Length];

        for (int i = 0; i < list.Length; i++)
        {
            result[i] = list[i].ToString();
        }

        return result;
    }
}
