using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>Wire format of the polyline returned for each route.</summary>
    public enum PolylineEncoding
    {
        /// <summary>Unspecified; the API uses <see cref="EncodedPolyline"/>.</summary>
        [EnumMember(Value = "POLYLINE_ENCODING_UNSPECIFIED")] Unspecified,

        /// <summary>
        /// Google's encoded polyline algorithm — same compact string format the legacy
        /// Directions API returns. Reusable with the library's existing polyline decoder.
        /// </summary>
        [EnumMember(Value = "ENCODED_POLYLINE")] EncodedPolyline,

        /// <summary>GeoJSON LineString representation.</summary>
        [EnumMember(Value = "GEO_JSON_LINESTRING")] GeoJsonLineString,
    }
}
