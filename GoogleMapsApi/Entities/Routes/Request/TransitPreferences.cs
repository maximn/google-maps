using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>Transit-specific preferences. Used only when <see cref="RoutesRequest.TravelMode"/> is <see cref="RoutesTravelMode.Transit"/>.</summary>
    public sealed class TransitPreferences
    {
        /// <summary>Preferred transit modes (subway, bus, ...). Empty means no preference.</summary>
        [JsonPropertyName("allowedTravelModes")]
        public List<TransitTravelMode>? AllowedTravelModes { get; set; }

        /// <summary>Optimization preference (e.g. fewer transfers).</summary>
        [JsonPropertyName("routingPreference")]
        public TransitRoutingPreference? RoutingPreference { get; set; }
    }

    /// <summary>Transit travel modes.</summary>
    public enum TransitTravelMode
    {
        /// <summary>Unspecified.</summary>
        [EnumMember(Value = "TRANSIT_TRAVEL_MODE_UNSPECIFIED")] Unspecified,

        /// <summary>Bus.</summary>
        [EnumMember(Value = "BUS")] Bus,

        /// <summary>Subway/metro.</summary>
        [EnumMember(Value = "SUBWAY")] Subway,

        /// <summary>Train (heavy rail).</summary>
        [EnumMember(Value = "TRAIN")] Train,

        /// <summary>Light rail / tram.</summary>
        [EnumMember(Value = "LIGHT_RAIL")] LightRail,

        /// <summary>Rail (any kind).</summary>
        [EnumMember(Value = "RAIL")] Rail,
    }

    /// <summary>Optimization preference for transit routing.</summary>
    public enum TransitRoutingPreference
    {
        /// <summary>Unspecified.</summary>
        [EnumMember(Value = "TRANSIT_ROUTING_PREFERENCE_UNSPECIFIED")] Unspecified,

        /// <summary>Prefer routes with less walking.</summary>
        [EnumMember(Value = "LESS_WALKING")] LessWalking,

        /// <summary>Prefer routes with fewer transfers.</summary>
        [EnumMember(Value = "FEWER_TRANSFERS")] FewerTransfers,
    }
}
