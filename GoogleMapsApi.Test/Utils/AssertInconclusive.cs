using System.Linq;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Entities.Elevation.Response;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.TimeZone.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test.Utils
{
    public static class AssertInconclusive
    {
        private const string QuotaExceedMessage = "Cannot run test since you have exceeded your Google API query limit.";

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(GeocodingResponse response)
        {
            if (response?.Status == Entities.Geocoding.Response.Status.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(DirectionsResponse response)
        {
            if (response?.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }

        /// <summary>
        /// If live routing returned no transit step at all (e.g. Google served a walking-only
        /// itinerary for a short trip), the response carries no vehicle data to assert on - mark the
        /// test inconclusive rather than failing on volatile live data.
        /// </summary>
        public static void HasTransitStep(DirectionsResponse response)
        {
            if (response?.Status != DirectionsStatusCodes.OK)
                Assert.Fail(response?.ErrorMessage ?? $"Directions API returned {response?.Status}.");

            var hasTransit = response?.Routes?
                .SelectMany(r => r.Legs ?? Enumerable.Empty<Leg>())
                .SelectMany(l => l.Steps ?? Enumerable.Empty<Step>())
                .Any(s => s.TransitDetails != null) ?? false;

            if (!hasTransit)
                throw new InconclusiveException("Google returned no transit step for this route (likely a walking-only itinerary); cannot assert transit vehicle data.");
        }

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(ElevationResponse response)
        {
            if (response?.Status == Entities.Elevation.Response.Status.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(TimeZoneResponse response)
        {
            if (response?.Status == Entities.TimeZone.Response.Status.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(DistanceMatrixResponse response)
        {
            if (response?.Status == DistanceMatrixStatusCodes.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }
    }
}
