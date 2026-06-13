using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Solar.Request;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>
    /// Response from the Solar API <c>dataLayers:get</c> endpoint — URLs to the raster data layers
    /// for a region. Each URL is fetched (with the API key appended) via a <see cref="GeoTiffRequest"/>.
    /// </summary>
    public sealed class DataLayersResponse : IResponseFor<DataLayersRequest>
    {
        /// <summary>Date the underlying imagery was captured.</summary>
        [JsonPropertyName("imageryDate")]
        public Date? ImageryDate { get; set; }

        /// <summary>Date the underlying imagery was processed.</summary>
        [JsonPropertyName("imageryProcessedDate")]
        public Date? ImageryProcessedDate { get; set; }

        /// <summary>URL to the digital surface model (DSM) GeoTIFF.</summary>
        [JsonPropertyName("dsmUrl")]
        public string? DsmUrl { get; set; }

        /// <summary>URL to the aerial RGB image GeoTIFF.</summary>
        [JsonPropertyName("rgbUrl")]
        public string? RgbUrl { get; set; }

        /// <summary>URL to the building mask GeoTIFF (which pixels are part of a roof).</summary>
        [JsonPropertyName("maskUrl")]
        public string? MaskUrl { get; set; }

        /// <summary>URL to the annual solar flux GeoTIFF (yearly sunlight per roof pixel).</summary>
        [JsonPropertyName("annualFluxUrl")]
        public string? AnnualFluxUrl { get; set; }

        /// <summary>URL to the monthly solar flux GeoTIFF (a band per month).</summary>
        [JsonPropertyName("monthlyFluxUrl")]
        public string? MonthlyFluxUrl { get; set; }

        /// <summary>URLs to the hourly shade GeoTIFFs, one entry per month of the year.</summary>
        [JsonPropertyName("hourlyShadeUrls")]
        public List<string>? HourlyShadeUrls { get; set; }

        /// <summary>Quality of the imagery used to produce these layers.</summary>
        [JsonPropertyName("imageryQuality")]
        public ImageryQuality ImageryQuality { get; set; }
    }
}
