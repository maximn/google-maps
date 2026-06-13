using System;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Solar.Request
{
    /// <summary>
    /// Request for the Google Solar API <c>buildingInsights:findClosest</c> endpoint
    /// (<c>GET https://solar.googleapis.com/v1/buildingInsights:findClosest</c>). Returns solar
    /// potential, roof geometry, panel layouts and financial analyses for the building closest to
    /// the supplied coordinate.
    /// </summary>
    /// <remarks>The Solar API is billable; calls beyond the free tier incur charges.</remarks>
    public sealed class BuildingInsightsRequest : MapsBaseRequest
    {
        /// <summary>Latitude of the query point, in degrees. Required.</summary>
        public double Latitude { get; set; }

        /// <summary>Longitude of the query point, in degrees. Required.</summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Minimum imagery quality the result must meet. When null the API returns the best
        /// available data regardless of quality.
        /// </summary>
        public ImageryQuality? RequiredQuality { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Solar API.");
            if (!IsSSL)
                throw new ArgumentException("Solar API requires SSL [IsSSL = true].");

            var url =
                "https://solar.googleapis.com/v1/buildingInsights:findClosest" +
                $"?location.latitude={CoordinateFormatter.Format(Latitude)}" +
                $"&location.longitude={CoordinateFormatter.Format(Longitude)}" +
                $"&key={Uri.EscapeDataString(ApiKey!)}";

            if (RequiredQuality.HasValue)
                url += $"&requiredQuality={EnumMemberHelper.GetValue(RequiredQuality.Value)}";

            return new Uri(url);
        }
    }
}
