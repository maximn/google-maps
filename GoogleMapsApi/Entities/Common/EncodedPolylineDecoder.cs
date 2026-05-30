using System;
using System.Collections.Generic;
using GoogleMapsApi.Entities.Directions.Response;

namespace GoogleMapsApi.Entities.Common
{
    /// <summary>
    /// Decodes Google's encoded-polyline string format into a sequence of <see cref="Location"/>s.
    /// See <see href="https://developers.google.com/maps/documentation/utilities/polylinealgorithm"/>.
    /// </summary>
    internal static class EncodedPolylineDecoder
    {
        public static IReadOnlyList<Location> Decode(string? encoded)
        {
            if (string.IsNullOrEmpty(encoded))
                return Array.Empty<Location>();

            try
            {
                var poly = new List<Location>();
                int index = 0;
                int lat = 0;
                int lng = 0;

                while (index < encoded!.Length)
                {
                    int b, shift = 0, result = 0;
                    do
                    {
                        b = encoded[index++] - 63;
                        result |= (b & 0x1f) << shift;
                        shift += 5;
                    } while (b >= 0x20);
                    int dlat = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
                    lat += dlat;

                    shift = 0;
                    result = 0;
                    do
                    {
                        b = encoded[index++] - 63;
                        result |= (b & 0x1f) << shift;
                        shift += 5;
                    } while (b >= 0x20);
                    int dlng = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
                    lng += dlng;

                    poly.Add(new Location(lat / 1E5, lng / 1E5));
                }

                return poly;
            }
            catch (Exception ex)
            {
                throw new PointsDecodingException("Couldn't decode points", encoded!, ex);
            }
        }
    }
}
