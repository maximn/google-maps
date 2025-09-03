using System.Linq;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using NUnit.Framework;
using GoogleMapsApi.Test.Utils;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesDetailsAddressComponentTests : BaseTestIntegration
    {
        private const string GoogleSydneyOfficePlaceId = "ChIJN1t_tDeuEmsRUsoyG83frY4";

        [Test]
        public async Task CanExtractStreetAddressFromPlacesDetails()
        {
            // Test with a known place that has a street address
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GoogleSydneyOfficePlaceId
            };

            var result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            Assert.That(result.Result.AddressComponent, Is.Not.Null.And.Not.Empty);

            // Extract street address using the helper method
            var streetAddress = result.Result.GetStreetAddress();
            Assert.That(streetAddress, Is.Not.Null.And.Not.Empty);
            Assert.That(streetAddress, Does.Contain("Pirrama"));
        }

        [Test]
        public async Task CanExtractStateFromPlacesDetails()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GoogleSydneyOfficePlaceId
            };

            var result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));

            // Extract state using the helper method
            var state = result.Result.GetState();
            var stateShort = result.Result.GetState(useShortName: true);
            
            Assert.That(state, Is.Not.Null.And.Not.Empty);
            Assert.That(stateShort, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task CanExtractPostalCodeFromPlacesDetails()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GoogleSydneyOfficePlaceId
            };

            var result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));

            // Extract postal code using the helper method
            var postalCode = result.Result.GetPostalCode();
            Assert.That(postalCode, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task CanExtractCompleteAddressBreakdown()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GoogleSydneyOfficePlaceId
            };

            var result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));

            // Extract complete address breakdown
            var addressBreakdown = result.Result.GetAddressBreakdown();
            
            Assert.That(addressBreakdown, Is.Not.Null);
            Assert.That(addressBreakdown.StreetAddress, Is.Not.Null.And.Not.Empty);
            Assert.That(addressBreakdown.City, Is.Not.Null.And.Not.Empty);
            Assert.That(addressBreakdown.State, Is.Not.Null.And.Not.Empty);
            Assert.That(addressBreakdown.PostalCode, Is.Not.Null.And.Not.Empty);
            Assert.That(addressBreakdown.Country, Is.Not.Null.And.Not.Empty);

            // Test the ToString method
            var formattedAddress = addressBreakdown.ToString();
            Assert.That(formattedAddress, Is.Not.Null.And.Not.Empty);
            Assert.That(formattedAddress, Does.Contain(","));
        }

        [Test]
        public async Task CanExtractCityFromPlacesDetails()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GoogleSydneyOfficePlaceId
            };

            var result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));

            // Extract city using the helper method
            var city = result.Result.GetCity();
            Assert.That(city, Is.Not.Null.And.Not.Empty);
            // Google Sydney office is in Pyrmont, which is a suburb of Sydney
            Assert.That(city, Is.EqualTo("Pyrmont"));
        }

        [Test]
        public async Task CanExtractCountryFromPlacesDetails()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GoogleSydneyOfficePlaceId
            };

            var result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));

            // Extract country using the helper method
            var country = result.Result.GetCountry();
            var countryShort = result.Result.GetCountry(useShortName: true);
            
            Assert.That(country, Is.Not.Null.And.Not.Empty);
            Assert.That(countryShort, Is.Not.Null.And.Not.Empty);
            Assert.That(countryShort, Is.EqualTo("AU"));
        }
    }
}
