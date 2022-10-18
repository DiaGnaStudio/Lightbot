using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DateExtensions 
{
    public static string ConvertSecondsToDate(this string seconds)
    {
        return ConvertSecondsToDate(Convert.ToDouble(seconds));
    }

    public static string ConvertSecondsToDate(this float seconds)
    {
        return ConvertSecondsToDate(Convert.ToDouble(seconds));
    }

    public static string ConvertSecondsToDate(this double seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);

        if (t.Days > 0)
            return t.ToString(@"d\d\,\ hh\:mm\:ss");
        return t.ToString(@"hh\:mm\:ss");
    }
}
