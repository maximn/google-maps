using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.AddressValidation.Request;
using GoogleMapsApi.Entities.AddressValidation.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class AddressValidationTests : BaseTestIntegration
    {
        [Test]
        public async Task Validate_KnownGoodUsAddress_ReturnsConfirmedVerdict()
        {
            var request = new AddressValidationRequest
            {
                ApiKey = ApiKey,
                Address = new PostalAddress
                {
                    RegionCode = "US",
                    AddressLines = new List<string> { "1600 Amphitheatre Pkwy" },
                    Locality = "Mountain View",
                    AdministrativeArea = "CA",
                    PostalCode = "94043",
                },
            };

            var response = await GoogleMaps.AddressValidation.QueryAsync(request);

            Assert.That(response.Result, Is.Not.Null);
            Assert.That(response.ResponseId, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Result!.Verdict!.AddressComplete, Is.True);
            Assert.That(response.Result.Address!.FormattedAddress, Does.Contain("1600"));
            Assert.That(response.Result.Geocode!.Location, Is.Not.Null);
            Assert.That(response.Result.Geocode.PlaceId, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Validate_GibberishAddress_ReturnsLowConfidenceVerdict()
        {
            var request = new AddressValidationRequest
            {
                ApiKey = ApiKey,
                Address = new PostalAddress
                {
                    RegionCode = "US",
                    AddressLines = new List<string> { "asdf qwer zzzz" },
                },
            };

            var response = await GoogleMaps.AddressValidation.QueryAsync(request);

            Assert.That(response.Result, Is.Not.Null);
            Assert.That(
                response.Result!.Verdict!.AddressComplete, Is.False,
                "A gibberish address should not be considered complete.");
        }

        [Test]
        public async Task Validate_WithUspsCass_ReturnsUspsData()
        {
            var request = new AddressValidationRequest
            {
                ApiKey = ApiKey,
                EnableUspsCass = true,
                Address = new PostalAddress
                {
                    RegionCode = "US",
                    AddressLines = new List<string> { "1600 Amphitheatre Pkwy" },
                    Locality = "Mountain View",
                    AdministrativeArea = "CA",
                    PostalCode = "94043",
                },
            };

            var response = await GoogleMaps.AddressValidation.QueryAsync(request);

            Assert.That(response.Result!.UspsData, Is.Not.Null, "USPS CASS data should be populated for a US address with EnableUspsCass=true.");
            Assert.That(response.Result.UspsData!.StandardizedAddress, Is.Not.Null);
            Assert.That(response.Result.UspsData.StandardizedAddress!.State, Is.EqualTo("CA"));
        }

        [Test]
        public async Task Validate_RoundTrip_PreviousResponseId_GroupsCallsIntoSession()
        {
            var first = await GoogleMaps.AddressValidation.QueryAsync(new AddressValidationRequest
            {
                ApiKey = ApiKey,
                Address = new PostalAddress
                {
                    RegionCode = "US",
                    AddressLines = new List<string> { "1600 Amphitheater Parkwy" },
                    Locality = "Mountain View",
                    AdministrativeArea = "CA",
                    PostalCode = "94043",
                },
            });

            Assert.That(first.ResponseId, Is.Not.Null.And.Not.Empty);

            var corrected = await GoogleMaps.AddressValidation.QueryAsync(new AddressValidationRequest
            {
                ApiKey = ApiKey,
                PreviousResponseId = first.ResponseId,
                Address = new PostalAddress
                {
                    RegionCode = "US",
                    AddressLines = new List<string> { "1600 Amphitheatre Pkwy" },
                    Locality = "Mountain View",
                    AdministrativeArea = "CA",
                    PostalCode = "94043",
                },
            });

            Assert.That(corrected.ResponseId, Is.Not.Null.And.Not.Empty);
            Assert.That(corrected.Result!.Verdict!.AddressComplete, Is.True);
        }
    }
}
