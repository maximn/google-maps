using System.Globalization;

namespace GoogleMapsApi.Engine
{
    /// <summary>
    /// Formats <see cref="double"/> values for Google Maps URLs: full precision, no scientific
    /// notation, trailing zeros trimmed (e.g. <c>37.4</c> rather than <c>37.39999999999999</c>).
    /// </summary>
    internal static class CoordinateFormatter
    {
        private static readonly string DoubleFormat = "0." + new string('#', 339);

        public static string Format(double value)
        {
            var formatted = value.ToString(DoubleFormat, CultureInfo.InvariantCulture);
            if (formatted.Contains("."))
                formatted = formatted.TrimEnd('0').TrimEnd('.');
            return formatted.Length == 0 ? "0" : formatted;
        }
    }
}
