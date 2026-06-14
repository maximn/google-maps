using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Solar.Request;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>
    /// Response from the Solar API <c>buildingInsights:findClosest</c> endpoint — solar potential
    /// and geometry for the building closest to the requested coordinate.
    /// </summary>
    public sealed class BuildingInsightsResponse : IResponseFor<BuildingInsightsRequest>
    {
        /// <summary>Resource name of the building, of the form <c>buildings/{place_id}</c>.</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>A point near the centre of the building.</summary>
        [JsonPropertyName("center")]
        public LatLng? Center { get; set; }

        /// <summary>The bounding box of the building.</summary>
        [JsonPropertyName("boundingBox")]
        public LatLngBox? BoundingBox { get; set; }

        /// <summary>Date the underlying imagery was captured.</summary>
        [JsonPropertyName("imageryDate")]
        public Date? ImageryDate { get; set; }

        /// <summary>Date the underlying imagery was processed.</summary>
        [JsonPropertyName("imageryProcessedDate")]
        public Date? ImageryProcessedDate { get; set; }

        /// <summary>Postal code (e.g. US ZIP code) the building is in.</summary>
        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        /// <summary>Administrative area 1 (e.g. US state) the building is in.</summary>
        [JsonPropertyName("administrativeArea")]
        public string? AdministrativeArea { get; set; }

        /// <summary>Statistical area (e.g. US census tract) the building is in.</summary>
        [JsonPropertyName("statisticalArea")]
        public string? StatisticalArea { get; set; }

        /// <summary>Region code (ISO 3166-1 alpha-2) of the country/region the building is in.</summary>
        [JsonPropertyName("regionCode")]
        public string? RegionCode { get; set; }

        /// <summary>The building's solar potential: panels, production and financials.</summary>
        [JsonPropertyName("solarPotential")]
        public SolarPotential? SolarPotential { get; set; }

        /// <summary>Quality of the imagery used to compute these results.</summary>
        [JsonPropertyName("imageryQuality")]
        public ImageryQuality ImageryQuality { get; set; }
    }
}
