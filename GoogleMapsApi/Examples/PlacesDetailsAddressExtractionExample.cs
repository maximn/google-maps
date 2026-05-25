using System;
using System.Net.Http;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;

namespace GoogleMapsApi.Examples
{
    /// <summary>
    /// Example demonstrating how to extract street address, state, and postal code from Places Details API results.
    /// This addresses GitHub issue #147: "How do I get the street address, state, and postal code from the place results?".
    /// </summary>
    /// <remarks>
    /// Construct one <see cref="GoogleMapsClient"/> per process (typically via DI / <c>IHttpClientFactory</c>)
    /// and reuse it across calls. The bare-<see cref="HttpClient"/> form shown here keeps the example self-contained.
    /// </remarks>
    public class PlacesDetailsAddressExtractionExample
    {
        private const string GoogleSydneyOfficePlaceId = "ChIJN1t_tDeuEmsRUsoyG83frY4";

        private readonly IGoogleMapsClient _maps;

        public PlacesDetailsAddressExtractionExample(IGoogleMapsClient maps)
        {
            _maps = maps ?? throw new ArgumentNullException(nameof(maps));
        }

        /// <summary>
        /// Factory for quick demo use: builds a client with a bare <see cref="HttpClient"/> and the supplied API key.
        /// In real code, inject <see cref="IGoogleMapsClient"/> via DI instead.
        /// </summary>
        public static PlacesDetailsAddressExtractionExample CreateForDemo(string apiKey)
        {
            var client = new GoogleMapsClient(new HttpClient(), new GoogleMapsClientOptions { ApiKey = apiKey });
            return new PlacesDetailsAddressExtractionExample(client);
        }

        /// <summary>
        /// Example showing how to extract individual address components.
        /// </summary>
        public async Task ExtractIndividualAddressComponents()
        {
            var request = new PlacesDetailsRequest { PlaceId = GoogleSydneyOfficePlaceId };
            var result = await _maps.PlacesDetails.QueryAsync(request);

            if (result.Status == Status.OK && result.Result?.AddressComponent != null)
            {
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
        /// Example showing how to extract a complete address breakdown.
        /// </summary>
        public async Task ExtractCompleteAddressBreakdown()
        {
            var request = new PlacesDetailsRequest { PlaceId = GoogleSydneyOfficePlaceId };
            var result = await _maps.PlacesDetails.QueryAsync(request);

            if (result.Status == Status.OK && result.Result?.AddressComponent != null)
            {
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
        /// Example showing how to extract address components for multiple places.
        /// </summary>
        public async Task ExtractAddressComponentsForMultiplePlaces()
        {
            var placeIds = new[]
            {
                "ChIJN1t_tDeuEmsRUsoyG83frY4", // Google Sydney
                "ChIJj61dQgK6j4AR4GeTYWZsKWw", // Google Mountain View
                "ChIJV4FfHcUZwokR5lP9_0s2OGI"  // Times Square, NYC
            };

            foreach (var placeId in placeIds)
            {
                var request = new PlacesDetailsRequest { PlaceId = placeId };
                var result = await _maps.PlacesDetails.QueryAsync(request);

                if (result.Status == Status.OK && result.Result?.AddressComponent != null)
                {
                    var addressBreakdown = result.Result.GetAddressBreakdown();
                    Console.WriteLine($"{result.Result.Name}: {addressBreakdown}");
                }
            }
        }
    }
}
