using System;
using System.Globalization;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Pollen.Request
{
    /// <summary>
    /// Request for the Google Pollen API <c>forecast:lookup</c> endpoint
    /// (<c>GET https://pollen.googleapis.com/v1/forecast:lookup</c>). Returns up to five days of daily
    /// pollen information (types and plants) for a coordinate.
    /// </summary>
    /// <remarks>The Pollen API is billable; calls beyond the free tier incur charges.</remarks>
    public sealed class PollenForecastRequest : MapsBaseRequest
    {
        /// <summary>Latitude of the query point, in degrees. Required.</summary>
        public double Latitude { get; set; }

        /// <summary>Longitude of the query point, in degrees. Required.</summary>
        public double Longitude { get; set; }

        /// <summary>Number of forecast days to return, in the range [1, 5]. Required.</summary>
        public int Days { get; set; }

        /// <summary>Maximum number of daily records per page. Defaults to the number of days when unset.</summary>
        public int? PageSize { get; set; }

        /// <summary>Page token from a previous response's <c>NextPageToken</c> to fetch the next page.</summary>
        public string? PageToken { get; set; }

        /// <summary>IETF BCP-47 language code for textual fields in the response (e.g. <c>"en"</c>).</summary>
        public string? LanguageCode { get; set; }

        /// <summary>
        /// Whether to include the detailed <c>PlantDescription</c> for each plant. Defaults to true
        /// server-side when unset; set false to reduce the response size.
        /// </summary>
        public bool? PlantsDescription { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Pollen API.");
            if (Days < 1 || Days > 5)
                throw new ArgumentException("Days must be in the range [1, 5].", nameof(Days));

            var url =
                "https://pollen.googleapis.com/v1/forecast:lookup" +
                $"?location.latitude={CoordinateFormatter.Format(Latitude)}" +
                $"&location.longitude={CoordinateFormatter.Format(Longitude)}" +
                $"&days={Days.ToString(CultureInfo.InvariantCulture)}" +
                $"&key={Uri.EscapeDataString(ApiKey!)}";

            if (PageSize.HasValue)
                url += $"&pageSize={PageSize.Value.ToString(CultureInfo.InvariantCulture)}";
            if (!string.IsNullOrWhiteSpace(PageToken))
                url += $"&pageToken={Uri.EscapeDataString(PageToken!)}";
            if (!string.IsNullOrWhiteSpace(LanguageCode))
                url += $"&languageCode={Uri.EscapeDataString(LanguageCode!)}";
            if (PlantsDescription.HasValue)
                url += $"&plantsDescription={(PlantsDescription.Value ? "true" : "false")}";

            return new Uri(url);
        }
    }
}
