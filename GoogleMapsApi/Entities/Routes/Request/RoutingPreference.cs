using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>
    /// How the Routes API should factor live and historical traffic into route computation.
    /// Only applicable when <see cref="RoutesRequest.TravelMode"/> is <see cref="RoutesTravelMode.Drive"/>
    /// or <see cref="RoutesTravelMode.TwoWheeler"/>.
    /// </summary>
    public enum RoutingPreference
    {
        /// <summary>Routing preference unspecified. Defaults to <see cref="TrafficUnaware"/>.</summary>
        [EnumMember(Value = "ROUTING_PREFERENCE_UNSPECIFIED")] Unspecified,

        /// <summary>Ignore live traffic. Cheapest and fastest, but less accurate during congestion.</summary>
        [EnumMember(Value = "TRAFFIC_UNAWARE")] TrafficUnaware,

        /// <summary>Use live traffic. Higher latency and cost than <see cref="TrafficUnaware"/>.</summary>
        [EnumMember(Value = "TRAFFIC_AWARE")] TrafficAware,

        /// <summary>Use live traffic with the highest accuracy. Highest latency and cost.</summary>
        [EnumMember(Value = "TRAFFIC_AWARE_OPTIMAL")] TrafficAwareOptimal,
    }
}
