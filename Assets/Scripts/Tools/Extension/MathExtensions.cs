public static class MathExtensions
{
    /// <summary>
    /// Remaps a value x in interval [A,B], to the proportional value in interval [C,D]
    /// </summary>
    /// <param name="x">The value to remap.</param>
    /// <param name="A">the minimum bound of interval [A,B] that contains the x value</param>
    /// <param name="B">the maximum bound of interval [A,B] that contains the x value</param>
    /// <param name="C">the minimum bound of target interval [C,D]</param>
    /// <param name="D">the maximum bound of target interval [C,D]</param>
    public static float Remap(float x, float A, float B, float C, float D)
    {
        float remappedValue = C + (x - A) / (B - A) * (D - C);
        return remappedValue;
    }

    public static double ConvertRange(this int value, float originalStart, float originalEnd, float newStart, float newEnd)
    {
        double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
        return (newStart + ((value - originalStart) * scale));
    }

    public static double ConvertRange(this int value, float newStart, float newEnd)
    {
        return value.ConvertRange(int.MinValue, int.MaxValue, newStart, newEnd);
    }

    public static double ConvertRange(this float value, float originalStart, float originalEnd, float newStart, float newEnd)
    {
        double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
        return (newStart + ((value - originalStart) * scale));
    }

    public static double ConvertRange(this float value, float newStart, float newEnd)
    {
        return value.ConvertRange(int.MinValue, int.MaxValue, newStart, newEnd);
    }
}
