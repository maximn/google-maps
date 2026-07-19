using System;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Solar.Request
{
    /// <summary>
    /// Request for the Google Solar API <c>dataLayers:get</c> endpoint
    /// (<c>GET https://solar.googleapis.com/v1/dataLayers:get</c>). Returns URLs to raster data
    /// layers (DSM, RGB, mask, annual/monthly flux, hourly shade) covering a region around a point.
    /// Download each layer with a <see cref="GeoTiffRequest"/>.
    /// </summary>
    /// <remarks>The Solar API is billable; calls beyond the free tier incur charges.</remarks>
    public sealed class DataLayersRequest : MapsBaseRequest
    {
        /// <summary>Latitude of the region centre, in degrees. Required.</summary>
        public double Latitude { get; set; }

        /// <summary>Longitude of the region centre, in degrees. Required.</summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Radius, in meters, defining the region the data should cover. Required; the API clamps
        /// values to the supported range (up to 100 m).
        /// </summary>
        public double RadiusMeters { get; set; }

        /// <summary>Which subset of layers to return. When null the API returns all layers.</summary>
        public DataLayerView? View { get; set; }

        /// <summary>Minimum imagery quality the result must meet.</summary>
        public ImageryQuality? RequiredQuality { get; set; }

        /// <summary>
        /// Minimum scale, in meters per pixel, of the returned data. Smaller values yield
        /// finer rasters. When null the API picks a default based on imagery quality.
        /// </summary>
        public double? PixelSizeMeters { get; set; }

        /// <summary>
        /// When true the API returns an error rather than falling back to lower-quality imagery
        /// if <see cref="RequiredQuality"/> cannot be met.
        /// </summary>
        public bool ExactQualityRequired { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Solar API.");
            if (RadiusMeters <= 0)
                throw new ArgumentException("RadiusMeters must be greater than zero.");

            var url =
                "https://solar.googleapis.com/v1/dataLayers:get" +
                $"?location.latitude={CoordinateFormatter.Format(Latitude)}" +
                $"&location.longitude={CoordinateFormatter.Format(Longitude)}" +
                $"&radiusMeters={CoordinateFormatter.Format(RadiusMeters)}" +
                $"&key={Uri.EscapeDataString(ApiKey!)}";

            if (View.HasValue)
                url += $"&view={EnumMemberHelper.GetValue(View.Value)}";
            if (RequiredQuality.HasValue)
                url += $"&requiredQuality={EnumMemberHelper.GetValue(RequiredQuality.Value)}";
            if (PixelSizeMeters.HasValue)
                url += $"&pixelSizeMeters={CoordinateFormatter.Format(PixelSizeMeters.Value)}";
            if (ExactQualityRequired)
                url += "&exactQualityRequired=true";

            return new Uri(url);
        }
    }
}
