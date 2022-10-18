using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string RemoveLastIndex(this string s)
    {
        return s.Remove(s.Length - 1, 1);
    }

    public static string FirstCharToUpper(this string input)
    {
        return input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };
    }

    public static IEnumerable<string> SplitCamelCase(this string source)
    {
        return Regex.Split(source, @"(?<!^)(?=[A-Z])");
    }

    public static IEnumerable<string> SplitInParts(this string s, int partLength)
    {
        if (s == null)
            throw new ArgumentNullException(nameof(s));
        if (partLength <= 0)
            throw new ArgumentException("Part length has to be positive.", nameof(partLength));

        for (var i = 0; i < s.Length; i += partLength)
            yield return s.Substring(i, Math.Min(partLength, s.Length - i));
    }

    public static string CreateTitle(this string title) => string.Join(" ", Regex.Replace(title, "(\\B[A-Z])", " $1")
                                                                                 .Split(' ')
                                                                                 .ToList()
                                                                                 .ConvertAll(word => word.Substring(0, 1).ToUpper() + word.Substring(1)));
}