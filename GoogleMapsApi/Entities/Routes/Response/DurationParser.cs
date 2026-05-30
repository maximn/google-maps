using System.Globalization;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>
    /// Parses protobuf-duration strings ("123s", "12.5s") returned by the Routes API.
    /// </summary>
    internal static class DurationParser
    {
        public static double? ToSeconds(string? raw)
        {
            if (string.IsNullOrEmpty(raw))
                return null;

            var trimmed = raw!.EndsWith("s") ? raw.Substring(0, raw.Length - 1) : raw;
            return double.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out var seconds)
                ? seconds
                : (double?)null;
        }
    }
}
