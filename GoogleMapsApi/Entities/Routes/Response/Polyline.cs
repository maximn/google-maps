using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>
    /// Polyline returned by the Routes API. By default the API returns the encoded-polyline
    /// format (same as the legacy Directions API); use the <see cref="DecodedPoints"/>
    /// helper to expand it into a list of <see cref="Location"/>s.
    /// </summary>
    public sealed class Polyline
    {
        /// <summary>Encoded polyline string. See <see cref="DecodedPoints"/>.</summary>
        [JsonPropertyName("encodedPolyline")]
        public string? EncodedPolyline { get; set; }

        /// <summary>
        /// Raw GeoJSON LineString returned when
        /// <see cref="Request.RoutesRequest.PolylineEncoding"/> is set to
        /// <see cref="Request.PolylineEncoding.GeoJsonLineString"/>. Decoding is left to the
        /// caller (any GeoJSON library will do).
        /// </summary>
        [JsonPropertyName("geoJsonLinestring")]
        public object? GeoJsonLinestring { get; set; }

        private Lazy<IReadOnlyList<Location>>? _pointsLazy;

        /// <summary>
        /// Decoded points from <see cref="EncodedPolyline"/>. Empty when no encoded polyline is set.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<Location> DecodedPoints =>
            (_pointsLazy ??= new Lazy<IReadOnlyList<Location>>(() => EncodedPolylineDecoder.Decode(EncodedPolyline))).Value;
    }
}
