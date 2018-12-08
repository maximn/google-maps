using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Entities.Elevation.Response;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;
using GoogleMapsApi.Entities.Places.Response;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Entities.PlacesFind.Response;
using GoogleMapsApi.Entities.PlacesNearBy.Response;
using GoogleMapsApi.Entities.PlacesRadar.Response;
using GoogleMapsApi.Entities.PlacesText.Response;
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

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(PlaceAutocompleteResponse response)
        {
            if (response?.Status == Entities.PlaceAutocomplete.Response.Status.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(PlacesDetailsResponse response)
        {
            if (response?.Status == Entities.PlacesDetails.Response.Status.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(PlacesNearByResponse response)
        {
            if (response?.Status == Entities.PlacesNearBy.Response.Status.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(PlacesResponse response)
        {
            if (response?.Status == Entities.Places.Response.Status.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(PlacesTextResponse response)
        {
            if (response?.Status == Entities.PlacesText.Response.Status.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }

        /// <summary>
        /// If the response status indicates fail because of quota exceeded - mark test as inconclusive.
        /// </summary>
        public static void NotExceedQuota(PlacesFindResponse response)
        {
            if (response?.Status == Entities.PlacesFind.Response.Status.OVER_QUERY_LIMIT)
                throw new InconclusiveException(QuotaExceedMessage);
        }
    }
}
