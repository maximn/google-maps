using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Information about electric-vehicle charging available at a place.</summary>
    public sealed class EvChargeOptions
    {
        /// <summary>Total number of connectors at this place.</summary>
        [JsonPropertyName("connectorCount")]
        public int? ConnectorCount { get; set; }

        /// <summary>Connectors aggregated by type and charge rate.</summary>
        [JsonPropertyName("connectorAggregation")]
        public List<ConnectorAggregation>? ConnectorAggregation { get; set; }
    }

    /// <summary>EV connectors aggregated by type and maximum charge rate.</summary>
    public sealed class ConnectorAggregation
    {
        /// <summary>Connector type (e.g. <c>EV_CONNECTOR_TYPE_CCS_COMBO_2</c>).</summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>Maximum charge rate, in kilowatts, of connectors in this aggregation.</summary>
        [JsonPropertyName("maxChargeRateKw")]
        public double? MaxChargeRateKw { get; set; }

        /// <summary>Number of connectors in this aggregation.</summary>
        [JsonPropertyName("count")]
        public int? Count { get; set; }

        /// <summary>Number of connectors in this aggregation currently available.</summary>
        [JsonPropertyName("availableCount")]
        public int? AvailableCount { get; set; }

        /// <summary>Number of connectors in this aggregation currently out of service.</summary>
        [JsonPropertyName("outOfServiceCount")]
        public int? OutOfServiceCount { get; set; }
    }
}
