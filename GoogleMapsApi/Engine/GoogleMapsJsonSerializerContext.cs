using GoogleMapsApi.Engine.JsonConverters;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Entities.Elevation.Response;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;
using GoogleMapsApi.Entities.Places.Response;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Entities.PlacesFind.Response;
using GoogleMapsApi.Entities.PlacesNearBy.Response;
using GoogleMapsApi.Entities.PlacesText.Response;
using GoogleMapsApi.Entities.TimeZone.Response;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Engine
{
    [JsonSerializable(typeof(DirectionsResponse), TypeInfoPropertyName = "DirectionsResponse")]
    [JsonSerializable(typeof(DistanceMatrixResponse), TypeInfoPropertyName = "DistanceMatrixResponse")]
    [JsonSerializable(typeof(ElevationResponse), TypeInfoPropertyName = "ElevationResponse")]
    [JsonSerializable(typeof(GeocodingResponse), TypeInfoPropertyName = "GeocodingResponse")]
    [JsonSerializable(typeof(PlaceAutocompleteResponse), TypeInfoPropertyName = "PlaceAutocompleteResponse")]
    [JsonSerializable(typeof(PlacesResponse), TypeInfoPropertyName = "PlacesResponse")]
    [JsonSerializable(typeof(PlacesDetailsResponse), TypeInfoPropertyName = "PlacesDetailsResponse")]
    [JsonSerializable(typeof(PlacesFindResponse), TypeInfoPropertyName = "PlacesFindResponse")]
    [JsonSerializable(typeof(PlacesNearByResponse), TypeInfoPropertyName = "PlacesNearByResponse")]
    [JsonSerializable(typeof(PlacesTextResponse), TypeInfoPropertyName = "PlacesTextResponse")]
    [JsonSerializable(typeof(TimeZoneResponse), TypeInfoPropertyName = "TimeZoneResponse")]

    [JsonSerializable(typeof(GoogleMapsApi.Entities.Elevation.Response.Result), TypeInfoPropertyName = "ElevationResult")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.Geocoding.Response.Result), TypeInfoPropertyName = "GeocodingResult")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.Places.Response.Result), TypeInfoPropertyName = "PlacesResult")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesDetails.Response.Result), TypeInfoPropertyName = "PlacesDetailsResult")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesNearBy.Response.Result), TypeInfoPropertyName = "PlacesNearByResult")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesText.Response.Result), TypeInfoPropertyName = "PlacesTextResult")]

    [JsonSerializable(typeof(GoogleMapsApi.Entities.Elevation.Response.Status), TypeInfoPropertyName = "ElevationStatus")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.Geocoding.Response.Status), TypeInfoPropertyName = "GeocodingStatus")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlaceAutocomplete.Response.Status), TypeInfoPropertyName = "PlaceAutocompleteStatus")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.Places.Response.Status), TypeInfoPropertyName = "PlacesStatus")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesDetails.Response.Status), TypeInfoPropertyName = "PlacesDetailsStatus")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesFind.Response.Status), TypeInfoPropertyName = "PlacesFindStatus")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesNearBy.Response.Status), TypeInfoPropertyName = "PlacesNearByStatus")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesText.Response.Status), TypeInfoPropertyName = "PlacesTextStatus")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.TimeZone.Response.Status), TypeInfoPropertyName = "TimeZoneStatus")]

    [JsonSerializable(typeof(GoogleMapsApi.Entities.Geocoding.Response.Geometry), TypeInfoPropertyName = "GeocodingGeometry")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.Places.Response.Geometry), TypeInfoPropertyName = "PlacesGeometry")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesDetails.Response.Geometry), TypeInfoPropertyName = "PlacesDetailsGeometry")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesFind.Response.Geometry), TypeInfoPropertyName = "PlacesFindGeometry")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesNearBy.Response.Geometry), TypeInfoPropertyName = "PlacesNearByGeometry")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesText.Response.Geometry), TypeInfoPropertyName = "PlacesTextGeometry")]

    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesDetails.Response.OpeningHours), TypeInfoPropertyName = "PlacesDetailsOpeningHours")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.PlacesFind.Response.OpeningHours), TypeInfoPropertyName = "PlacesFindOpeningHours")]

    [JsonSerializable(typeof(GoogleMapsApi.Entities.Directions.Response.Distance), TypeInfoPropertyName = "DirectionsDistance")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.DistanceMatrix.Response.Distance), TypeInfoPropertyName = "DistanceMatrixDistance")]

    [JsonSerializable(typeof(GoogleMapsApi.Entities.Directions.Response.Duration), TypeInfoPropertyName = "DirectionsDuration")]
    [JsonSerializable(typeof(GoogleMapsApi.Entities.DistanceMatrix.Response.Duration), TypeInfoPropertyName = "DistanceMatrixDuration")]

    [JsonSerializable(typeof(IEnumerable<GoogleMapsApi.Entities.Elevation.Response.Result>), TypeInfoPropertyName = "IEnumerableElevationResult")]
    [JsonSerializable(typeof(IEnumerable<GoogleMapsApi.Entities.Geocoding.Response.Result>), TypeInfoPropertyName = "IEnumerableGeocodingResult")]
    [JsonSerializable(typeof(IEnumerable<GoogleMapsApi.Entities.Places.Response.Result>), TypeInfoPropertyName = "IEnumerablePlacesResult")]
    [JsonSerializable(typeof(IEnumerable<GoogleMapsApi.Entities.PlacesDetails.Response.Result>), TypeInfoPropertyName = "IEnumerablePlacesDetailsResult")]
    [JsonSerializable(typeof(IEnumerable<GoogleMapsApi.Entities.PlacesNearBy.Response.Result>), TypeInfoPropertyName = "IEnumerablePlacesNearByResult")]
    [JsonSerializable(typeof(IEnumerable<GoogleMapsApi.Entities.PlacesText.Response.Result>), TypeInfoPropertyName = "IEnumerablePlacesTextResult")]

    [JsonSerializable(typeof(object))]

    [JsonSourceGenerationOptions(
        PropertyNameCaseInsensitive = true,
        Converters = new[] {
            typeof(EnumMemberJsonConverterFactory),
            typeof(PriceLevelJsonConverter),
            typeof(OverviewPolylineJsonConverter),
            typeof(DurationJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Response.Duration>),
            typeof(DurationJsonConverter<GoogleMapsApi.Entities.Directions.Response.Duration>),
    })]
    public partial class GoogleMapsJsonSerializerContext : JsonSerializerContext
    {
    }
}