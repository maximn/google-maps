using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNew.Common;
using GoogleMapsApi.Entities.PlacesNew.Request;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>
    /// A place returned by the Places API (New). Place Details returns a bare instance of this type;
    /// the search endpoints return a list of them. Which fields are populated depends on the request's
    /// field mask — fields outside the mask are returned as null.
    /// </summary>
    public sealed class Place : IResponseFor<PlaceDetailsRequest>
    {
        /// <summary>Place ID (stable identifier for the place).</summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>Resource name of the place (e.g. <c>places/{placeId}</c>).</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Localized display name of the place.</summary>
        [JsonPropertyName("displayName")]
        public LocalizedText? DisplayName { get; set; }

        /// <summary>Full, human-readable address.</summary>
        [JsonPropertyName("formattedAddress")]
        public string? FormattedAddress { get; set; }

        /// <summary>Short, human-readable address.</summary>
        [JsonPropertyName("shortFormattedAddress")]
        public string? ShortFormattedAddress { get; set; }

        /// <summary>Structured address components.</summary>
        [JsonPropertyName("addressComponents")]
        public List<AddressComponent>? AddressComponents { get; set; }

        /// <summary>Open Location Code (plus code) for the place.</summary>
        [JsonPropertyName("plusCode")]
        public PlusCode? PlusCode { get; set; }

        /// <summary>Geographic location of the place.</summary>
        [JsonPropertyName("location")]
        public LatLng? Location { get; set; }

        /// <summary>Viewport suitable for displaying the place on a map.</summary>
        [JsonPropertyName("viewport")]
        public Viewport? Viewport { get; set; }

        /// <summary>Average rating, 1.0–5.0.</summary>
        [JsonPropertyName("rating")]
        public double? Rating { get; set; }

        /// <summary>Total number of user ratings.</summary>
        [JsonPropertyName("userRatingCount")]
        public int? UserRatingCount { get; set; }

        /// <summary>Place types associated with this place.</summary>
        [JsonPropertyName("types")]
        public List<string>? Types { get; set; }

        /// <summary>Primary type of the place.</summary>
        [JsonPropertyName("primaryType")]
        public string? PrimaryType { get; set; }

        /// <summary>Localized display name of the primary type.</summary>
        [JsonPropertyName("primaryTypeDisplayName")]
        public LocalizedText? PrimaryTypeDisplayName { get; set; }

        /// <summary>Operational status of the business.</summary>
        [JsonPropertyName("businessStatus")]
        public BusinessStatus? BusinessStatus { get; set; }

        /// <summary>Price level of the place.</summary>
        [JsonPropertyName("priceLevel")]
        public PriceLevel? PriceLevel { get; set; }

        /// <summary>URI to the place's Google Maps page.</summary>
        [JsonPropertyName("googleMapsUri")]
        public string? GoogleMapsUri { get; set; }

        /// <summary>URI to the place's authoritative website.</summary>
        [JsonPropertyName("websiteUri")]
        public string? WebsiteUri { get; set; }

        /// <summary>National-format phone number.</summary>
        [JsonPropertyName("nationalPhoneNumber")]
        public string? NationalPhoneNumber { get; set; }

        /// <summary>International-format phone number.</summary>
        [JsonPropertyName("internationalPhoneNumber")]
        public string? InternationalPhoneNumber { get; set; }

        /// <summary>Offset from UTC, in minutes, of the place's current timezone.</summary>
        [JsonPropertyName("utcOffsetMinutes")]
        public int? UtcOffsetMinutes { get; set; }

        /// <summary>Regular (standard) opening hours.</summary>
        [JsonPropertyName("regularOpeningHours")]
        public OpeningHours? RegularOpeningHours { get; set; }

        /// <summary>Current opening hours, including holiday/special-day overrides.</summary>
        [JsonPropertyName("currentOpeningHours")]
        public OpeningHours? CurrentOpeningHours { get; set; }

        /// <summary>Photos of the place.</summary>
        [JsonPropertyName("photos")]
        public List<Photo>? Photos { get; set; }

        /// <summary>User reviews of the place.</summary>
        [JsonPropertyName("reviews")]
        public List<Review>? Reviews { get; set; }

        /// <summary>Editorial summary of the place.</summary>
        [JsonPropertyName("editorialSummary")]
        public EditorialSummary? EditorialSummary { get; set; }

        /// <summary>Payment options accepted by the place.</summary>
        [JsonPropertyName("paymentOptions")]
        public PaymentOptions? PaymentOptions { get; set; }

        /// <summary>Parking options available at the place.</summary>
        [JsonPropertyName("parkingOptions")]
        public ParkingOptions? ParkingOptions { get; set; }

        /// <summary>Accessibility options available at the place.</summary>
        [JsonPropertyName("accessibilityOptions")]
        public AccessibilityOptions? AccessibilityOptions { get; set; }

        /// <summary>Fuel options (for fuel stations).</summary>
        [JsonPropertyName("fuelOptions")]
        public FuelOptions? FuelOptions { get; set; }

        /// <summary>EV charging options (for places with EV chargers).</summary>
        [JsonPropertyName("evChargeOptions")]
        public EvChargeOptions? EvChargeOptions { get; set; }
    }
}
