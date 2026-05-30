using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>
    /// Optional set of conditions that influence route generation — avoidances, vehicle info
    /// (for emissions-aware routing), and toll passes.
    /// </summary>
    public sealed class RouteModifiers
    {
        /// <summary>Avoid toll roads where reasonable.</summary>
        [JsonPropertyName("avoidTolls")]
        public bool? AvoidTolls { get; set; }

        /// <summary>Avoid highways where reasonable.</summary>
        [JsonPropertyName("avoidHighways")]
        public bool? AvoidHighways { get; set; }

        /// <summary>Avoid ferries where reasonable.</summary>
        [JsonPropertyName("avoidFerries")]
        public bool? AvoidFerries { get; set; }

        /// <summary>Avoid navigation indoors where reasonable. Walking only.</summary>
        [JsonPropertyName("avoidIndoor")]
        public bool? AvoidIndoor { get; set; }

        /// <summary>
        /// Vehicle information used for emissions-aware routing. Required when
        /// <see cref="RoutesRequest.ExtraComputations"/> includes
        /// <see cref="ExtraComputation.FuelConsumption"/>.
        /// </summary>
        [JsonPropertyName("vehicleInfo")]
        public VehicleInfo? VehicleInfo { get; set; }

        /// <summary>
        /// Toll passes the vehicle holds. When supplied, the Routes API may return routes
        /// behind toll-pass-only lanes.
        /// </summary>
        [JsonPropertyName("tollPasses")]
        public List<string>? TollPasses { get; set; }
    }

    /// <summary>Vehicle attributes used for emissions-aware routing.</summary>
    public sealed class VehicleInfo
    {
        /// <summary>Engine / emission type of the vehicle.</summary>
        [JsonPropertyName("emissionType")]
        public VehicleEmissionType? EmissionType { get; set; }
    }

    /// <summary>Vehicle emission/engine type.</summary>
    public enum VehicleEmissionType
    {
        /// <summary>Unspecified; treated as <see cref="Gasoline"/>.</summary>
        [EnumMember(Value = "VEHICLE_EMISSION_TYPE_UNSPECIFIED")] Unspecified,

        /// <summary>Gasoline / petrol engine.</summary>
        [EnumMember(Value = "GASOLINE")] Gasoline,

        /// <summary>Electric vehicle.</summary>
        [EnumMember(Value = "ELECTRIC")] Electric,

        /// <summary>Hybrid (gasoline + electric).</summary>
        [EnumMember(Value = "HYBRID")] Hybrid,

        /// <summary>Diesel engine.</summary>
        [EnumMember(Value = "DIESEL")] Diesel,
    }
}
