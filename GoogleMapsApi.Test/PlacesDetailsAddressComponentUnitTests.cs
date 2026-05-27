using System.Text.Json;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Unit tests for the address-component helper methods on <see cref="Result"/>.
    /// Uses a canned Places Details payload (Google Sydney office) so we don't hit the Places API
    /// quota on every CI run — the helpers only read data, no network round-trip needed.
    /// </summary>
    [TestFixture]
    public class PlacesDetailsAddressComponentUnitTests
    {
        private const string GoogleSydneyDetailsJson = """
        {
          "status": "OK",
          "result": {
            "place_id": "ChIJN1t_tDeuEmsRUsoyG83frY4",
            "name": "Google Workplace 6",
            "formatted_address": "48 Pirrama Rd, Pyrmont NSW 2009, Australia",
            "address_components": [
              { "long_name": "48",                "short_name": "48",   "types": ["street_number"] },
              { "long_name": "Pirrama Road",      "short_name": "Pirrama Rd", "types": ["route"] },
              { "long_name": "Pyrmont",           "short_name": "Pyrmont", "types": ["locality", "political"] },
              { "long_name": "Sydney",            "short_name": "Sydney",  "types": ["administrative_area_level_2", "political"] },
              { "long_name": "New South Wales",   "short_name": "NSW",   "types": ["administrative_area_level_1", "political"] },
              { "long_name": "Australia",         "short_name": "AU",    "types": ["country", "political"] },
              { "long_name": "2009",              "short_name": "2009",  "types": ["postal_code"] }
            ]
          }
        }
        """;

        private static PlacesDetailsResponse DeserializeSydneyDetails()
        {
            var options = JsonSerializerConfiguration.CreateOptions();
            var response = JsonSerializer.Deserialize<PlacesDetailsResponse>(GoogleSydneyDetailsJson, options);
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.Status, Is.EqualTo(Status.OK));
            Assert.That(response.Result.AddressComponent, Is.Not.Null.And.Not.Empty);
            return response;
        }

        [Test]
        public void GetStreetAddress_ReturnsStreetNumberAndRoute()
        {
            var result = DeserializeSydneyDetails().Result;

            var streetAddress = result.GetStreetAddress();

            Assert.That(streetAddress, Is.EqualTo("48 Pirrama Road"));
        }

        [Test]
        public void GetState_ReturnsLongAndShortNames()
        {
            var result = DeserializeSydneyDetails().Result;

            Assert.That(result.GetState(), Is.EqualTo("New South Wales"));
            Assert.That(result.GetState(useShortName: true), Is.EqualTo("NSW"));
        }

        [Test]
        public void GetPostalCode_ReturnsPostalCode()
        {
            var result = DeserializeSydneyDetails().Result;

            Assert.That(result.GetPostalCode(), Is.EqualTo("2009"));
        }

        [Test]
        public void GetCity_ReturnsLocality()
        {
            var result = DeserializeSydneyDetails().Result;

            Assert.That(result.GetCity(), Is.EqualTo("Pyrmont"));
        }

        [Test]
        public void GetCountry_ReturnsLongAndShortNames()
        {
            var result = DeserializeSydneyDetails().Result;

            Assert.That(result.GetCountry(), Is.EqualTo("Australia"));
            Assert.That(result.GetCountry(useShortName: true), Is.EqualTo("AU"));
        }

        [Test]
        public void GetAddressBreakdown_PopulatesAllFields_AndFormatsToString()
        {
            var result = DeserializeSydneyDetails().Result;

            var breakdown = result.GetAddressBreakdown();

            Assert.That(breakdown.StreetAddress, Is.EqualTo("48 Pirrama Road"));
            Assert.That(breakdown.City, Is.EqualTo("Pyrmont"));
            Assert.That(breakdown.State, Is.EqualTo("New South Wales"));
            Assert.That(breakdown.PostalCode, Is.EqualTo("2009"));
            Assert.That(breakdown.Country, Is.EqualTo("Australia"));
            Assert.That(breakdown.ToString(), Is.EqualTo("48 Pirrama Road, Pyrmont, New South Wales, 2009, Australia"));
        }
    }
}
