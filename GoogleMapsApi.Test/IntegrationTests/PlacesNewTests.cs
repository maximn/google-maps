using System.Linq;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.PlacesNew.Common;
using GoogleMapsApi.Entities.PlacesNew.Request;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    [BillableTest]
    public class PlacesNewTests : BaseTestIntegration
    {
        [Test]
        public async Task SearchText_ReturnsPlaces()
        {
            var request = new SearchTextRequest
            {
                ApiKey = ApiKey,
                TextQuery = "pizza in New York",
            };

            var response = await GoogleMaps.PlacesSearchText.QueryAsync(request);

            Assert.That(response.Places, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Places![0].DisplayName!.Text, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task SearchNearby_ReturnsPlaces()
        {
            var request = new SearchNearbyRequest
            {
                ApiKey = ApiKey,
                IncludedTypes = new System.Collections.Generic.List<string> { "restaurant" },
                MaxResultCount = 5,
                LocationRestriction = new LocationRestriction
                {
                    Circle = new Circle
                    {
                        Center = new LatLng { Latitude = 40.7580, Longitude = -73.9855 },
                        Radius = 500,
                    },
                },
            };

            var response = await GoogleMaps.PlacesSearchNearby.QueryAsync(request);

            Assert.That(response.Places, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Details_ByPlaceId_ReturnsPlace()
        {
            var search = await GoogleMaps.PlacesSearchText.QueryAsync(new SearchTextRequest
            {
                ApiKey = ApiKey,
                TextQuery = "Empire State Building",
            });
            var placeId = search.Places![0].Id!;

            var place = await GoogleMaps.PlaceDetailsNew.QueryAsync(new PlaceDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = placeId,
            });

            Assert.That(place.Id, Is.EqualTo(placeId));
            Assert.That(place.DisplayName!.Text, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Autocomplete_ReturnsSuggestions()
        {
            var request = new AutocompleteRequest
            {
                ApiKey = ApiKey,
                Input = "Eiffel Tow",
            };

            var response = await GoogleMaps.PlacesAutocompleteNew.QueryAsync(request);

            Assert.That(response.Suggestions, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Photo_ResolvesToUri()
        {
            var details = await GoogleMaps.PlaceDetailsNew.QueryAsync(new PlaceDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = (await GoogleMaps.PlacesSearchText.QueryAsync(new SearchTextRequest
                {
                    ApiKey = ApiKey,
                    TextQuery = "Statue of Liberty",
                })).Places![0].Id!,
                FieldMask = "id,displayName,photos",
            });

            var photoName = details.Photos!.First().Name!;

            var photo = await GoogleMaps.PlacePhoto.QueryAsync(new PlacePhotoRequest
            {
                ApiKey = ApiKey,
                PhotoName = photoName,
                MaxWidthPx = 400,
            });

            Assert.That(photo.PhotoUri, Is.Not.Null.And.Not.Empty);
        }
    }
}
