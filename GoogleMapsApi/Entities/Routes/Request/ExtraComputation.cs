using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>
    /// Optional extra computations the Routes API can perform alongside the route. Each entry
    /// incurs additional billing — request only what you need.
    /// </summary>
    public enum ExtraComputation
    {
        /// <summary>Unspecified; the API ignores this value.</summary>
        [EnumMember(Value = "EXTRA_COMPUTATION_UNSPECIFIED")] Unspecified,

        /// <summary>Include toll information for the route.</summary>
        [EnumMember(Value = "TOLLS")] Tolls,

        /// <summary>Include estimated fuel consumption for the route.</summary>
        [EnumMember(Value = "FUEL_CONSUMPTION")] FuelConsumption,

        /// <summary>Include traffic-aware polylines colored by congestion level.</summary>
        [EnumMember(Value = "TRAFFIC_ON_POLYLINE")] TrafficOnPolyline,

        /// <summary>Include HTML-formatted navigation instructions on each leg/step.</summary>
        [EnumMember(Value = "HTML_FORMATTED_NAVIGATION_INSTRUCTIONS")] HtmlFormattedNavigationInstructions,
    }

    /// <summary>Reference route to compute alongside the default one.</summary>
    public enum ReferenceRoute
    {
        /// <summary>Unspecified; the API ignores this value.</summary>
        [EnumMember(Value = "REFERENCE_ROUTE_UNSPECIFIED")] Unspecified,

        /// <summary>Compute a fuel-efficient alternative route.</summary>
        [EnumMember(Value = "FUEL_EFFICIENT")] FuelEfficient,
    }

    /// <summary>
    /// Traffic model used when <see cref="RoutesRequest.RoutingPreference"/> is
    /// <see cref="Request.RoutingPreference.TrafficAware"/> or
    /// <see cref="Request.RoutingPreference.TrafficAwareOptimal"/>.
    /// </summary>
    public enum TrafficModel
    {
        /// <summary>Unspecified; treated as <see cref="BestGuess"/>.</summary>
        [EnumMember(Value = "TRAFFIC_MODEL_UNSPECIFIED")] Unspecified,

        /// <summary>Use the best estimate of travel time given live and historical traffic.</summary>
        [EnumMember(Value = "BEST_GUESS")] BestGuess,

        /// <summary>Use the pessimistic estimate (longer than actual in most cases).</summary>
        [EnumMember(Value = "PESSIMISTIC")] Pessimistic,

        /// <summary>Use the optimistic estimate (shorter than actual in most cases).</summary>
        [EnumMember(Value = "OPTIMISTIC")] Optimistic,
    }
}
