using System;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;

namespace GoogleMapsApi.Examples
{
    /// <summary>
    /// Example demonstrating how to extract street address, state, and postal code from Places Details API results
    /// This addresses GitHub issue #147: "How do i get the street address, state, and postal code from the place results?"
    /// </summary>
    public class PlacesDetailsAddressExtractionExample
    {
        private const string GoogleSydneyOfficePlaceId = "ChIJN1t_tDeuEmsRUsoyG83frY4";

        /// <summary>
        /// Example showing how to extract individual address components
        /// </summary>
        public static async Task ExtractIndividualAddressComponents()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = "YOUR_API_KEY_HERE",
                PlaceId = GoogleSydneyOfficePlaceId
            };

            var result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            if (result.Status == Status.OK && result.Result?.AddressComponent != null)
            {
                // Extract individual address components using helper methods
                var streetAddress = result.Result.GetStreetAddress();
                var state = result.Result.GetState();
                var stateShort = result.Result.GetState(useShortName: true);
                var postalCode = result.Result.GetPostalCode();
                var city = result.Result.GetCity();
                var country = result.Result.GetCountry();

                Console.WriteLine($"Street Address: {streetAddress}");
                Console.WriteLine($"City: {city}");
                Console.WriteLine($"State: {state} ({stateShort})");
                Console.WriteLine($"Postal Code: {postalCode}");
                Console.WriteLine($"Country: {country}");
            }
        }

        /// <summary>
        /// Example showing how to extract complete address breakdown
        /// </summary>
        public static async Task ExtractCompleteAddressBreakdown()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = "YOUR_API_KEY_HERE",
                PlaceId = GoogleSydneyOfficePlaceId
            };

            var result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            if (result.Status == Status.OK && result.Result?.AddressComponent != null)
            {
                // Extract complete address breakdown
                var addressBreakdown = result.Result.GetAddressBreakdown();

                Console.WriteLine($"Complete Address: {addressBreakdown}");
                Console.WriteLine($"Street: {addressBreakdown.StreetAddress}");
                Console.WriteLine($"City: {addressBreakdown.City}");
                Console.WriteLine($"State: {addressBreakdown.State}");
                Console.WriteLine($"Postal Code: {addressBreakdown.PostalCode}");
                Console.WriteLine($"Country: {addressBreakdown.Country}");
            }
        }

        /// <summary>
        /// Example showing how to extract address components for multiple places
        /// </summary>
        public static async Task ExtractAddressComponentsForMultiplePlaces()
        {
            var placeIds = new[]
            {
                "ChIJN1t_tDeuEmsRUsoyG83frY4", // Google Sydney
                "ChIJj61dQgK6j4AR4GeTYWZsKWw", // Google Mountain View
                "ChIJV4FfHcUZwokR5lP9_0s2OGI"  // Times Square, NYC
            };

            foreach (var placeId in placeIds)
            {
                var request = new PlacesDetailsRequest
                {
                    ApiKey = "YOUR_API_KEY_HERE",
                    PlaceId = placeId
                };

                var result = await GoogleMaps.PlacesDetails.QueryAsync(request);

                if (result.Status == Status.OK && result.Result?.AddressComponent != null)
                {
                    var addressBreakdown = result.Result.GetAddressBreakdown();
                    Console.WriteLine($"{result.Result.Name}: {addressBreakdown}");
                }
            }
        }
    }
}
