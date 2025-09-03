using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    [DataContract]
    public class Result
    {
        /// <summary>
        /// name contains the human-readable name for the returned result. For establishment results, this is usually the canonicalized business name.
        /// </summary>
    
        /// <summary>
        /// address_components is an array containing the separate address components.
        /// Use the helper methods on this Result class to easily extract street address, state, postal code, etc.
        /// Example: result.GetStreetAddress(), result.GetState(), result.GetPostalCode()
        /// </summary>
        [DataMember(Name="address_components")]
        public IEnumerable<GoogleMapsApi.Entities.Geocoding.Response.AddressComponent> AddressComponent { get; set; }

        /// <summary>
        /// A representation of the place's address in the adr microformat.
        /// </summary>
        [DataMember(Name="adr_address")]
        public string AdrAddress { get; set; }

        /// <summary>
        /// Indicates the operational status of the place, if it is a business.
        /// </summary>
        [DataMember(Name="business_status")]
        public BusinessStatus? BusinessStatus { get; set; }

        /// <summary>
        /// Specifies if the business supports curbside pickup.
        /// </summary>
        [DataMember(Name = "curbside_pickup")]
        public bool? CurbsidePickup { get; set; }

        /// <summary>
        /// Contains the hours of operation for the next seven days (including today).
        /// </summary>
        [DataMember(Name = "current_opening_hours")]
        public OpeningHours CurrentOpeningHours { get; set; }

        /// <summary>
        /// Specifies if the business supports delivery.
        /// </summary>
        [DataMember(Name = "delivery")]
        public bool? Delivery { get; set; }

        /// <summary>
        /// Specifies if the business supports indoor or outdoor seating options.
        /// </summary>
        [DataMember(Name = "dine_in")]
        public bool? DineIn { get; set; }

        /// <summary>
        /// Contains a summary of the place.
        /// </summary>
        [DataMember(Name = "editorial_summary")]
        public PlaceEditorialSummary EditorialSummary { get; set; }


        [DataMember(Name = "events")]
        public IEnumerable<Event> Event { get; set; }

        [DataMember(Name = "formatted_address")]
        public string FormattedAddress { get; set; }

        [DataMember(Name = "formatted_phone_number")]
        public string FormattedPhoneNumber { get; set; }

        [DataMember(Name = "geometry")]
        public Geometry Geometry { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Contains the default HEX color code for the place's category.
        /// </summary>
        [DataMember(Name = "icon_background_color")]
        public string IconBackgroundColor { get; set; }

        /// <summary>
        /// Contains the URL of a recommended icon, minus the .svg or .png file type extension.
        /// </summary>
        [DataMember(Name = "icon_mask_base_uri")]
        public string IconMaskBaseUri { get; set; }

        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "international_phone_number")]
        public string InternationalPhoneNumber { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Opening hours information
        /// </summary>
        [DataMember(Name = "opening_hours")]
        public OpeningHours OpeningHours { get; set; }

        [DataMember(Name = "permanently_closed")]
        [Obsolete("Use BusinessStatus property instead. See https://developers.google.com/maps/documentation/places/web-service/details#fields for more information.")]
        public bool PermanentlyClosed { get; set; }

        /// <summary>
        /// An encoded location reference, derived from latitude and longitude coordinates.
        /// </summary>
        [DataMember(Name = "plus_code")]
        public PlusCode PlusCode { get; set; }

        [DataMember(Name = "photos")]
        public IEnumerable<Photo> Photos { get; set; }

        public PriceLevel? PriceLevel;

        [DataMember(Name = "price_level")]
        internal string string_PriceLevel
        {
            get { return PriceLevel.HasValue ? ((int) PriceLevel).ToString(CultureInfo.InvariantCulture) : null; }
            set
            {
                if (value == null)
                    PriceLevel = null;
                else
                {
                    int priceLevelInt;
                    if (int.TryParse(value, out priceLevelInt))
                        PriceLevel = (PriceLevel) priceLevelInt;
                    else
                        PriceLevel = null;
                }
            }
        }

        [DataMember(Name = "rating")]
        public double Rating { get; set; }

        [DataMember(Name = "reference")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
        public string Reference { get; set; }

        [DataMember(Name = "reviews")]
        public IEnumerable<Review> Review { get; set; }

        /// <summary>
        /// Contains an array of entries for the next seven days including information about secondary hours of a business.
        /// </summary>
        [DataMember(Name = "secondary_opening_hours")]
        public IEnumerable<OpeningHours> SecondaryOpeningHours { get; set; }

        /// <summary>
        /// Specifies if the place serves beer.
        /// </summary>
        [DataMember(Name = "serves_beer")]
        public bool? ServesBeer { get; set; }

        /// <summary>
        /// Specifies if the place serves breakfast.
        /// </summary>
        [DataMember(Name = "serves_breakfast")]
        public bool? ServesBreakfast { get; set; }

        /// <summary>
        /// Specifies if the place serves brunch.
        /// </summary>
        [DataMember(Name = "serves_brunch")]
        public bool? ServesBrunch { get; set; }

        /// <summary>
        /// Specifies if the place serves dinner.
        /// </summary>
        [DataMember(Name = "serves_dinner")]
        public bool? ServesDinner { get; set; }

        /// <summary>
        /// Specifies if the place serves lunch.
        /// </summary>
        [DataMember(Name = "serves_lunch")]
        public bool? ServesLunch { get; set; }

        /// <summary>
        /// Specifies if the place serves vegetarian food.
        /// </summary>
        [DataMember(Name = "serves_vegetarian_food")]
        public bool? ServesVegetarianFood { get; set; }

        /// <summary>
        /// Specifies if the place serves wine.
        /// </summary>
        [DataMember(Name = "serves_wine")]
        public bool? ServesWine { get; set; }

        /// <summary>
        /// Specifies if the business supports takeout.
        /// </summary>
        [DataMember(Name = "takeout")]
        public bool? Takeout { get; set; }

        [DataMember(Name = "types")]
        public string[] Types { get; set; }

        [DataMember(Name = "url")]
        public string URL { get; set; }

        /// <summary>
        /// The total number of reviews, with or without text, for this place.
        /// </summary>
        [DataMember(Name = "user_ratings_total")]
        public int? UserRatingsTotal { get; set; }

        [DataMember(Name = "utc_offset")]
        public string UTCOffset { get; set; }

        [DataMember(Name = "vicinity")]
        public string Vicinity { get; set; }

        [DataMember(Name = "website")]
        public string Website { get; set; }

        /// <summary>
        /// Specifies if the place has an entrance that is wheelchair-accessible.
        /// </summary>
        [DataMember(Name = "wheelchair_accessible_entrance")]
        public bool? WheelchairAccessibleEntrance { get; set; }

        [DataMember(Name = "place_id")]
        public string PlaceId { get; set; }

        #region Address Component Helper Methods

        /// <summary>
        /// Gets the street address (street number + route) from address components
        /// </summary>
        /// <returns>Street address or null if not found</returns>
        public string GetStreetAddress()
        {
            if (AddressComponent == null)
                return null;

            var streetNumber = GetAddressComponentByType("street_number")?.LongName;
            var route = GetAddressComponentByType("route")?.LongName;

            if (string.IsNullOrEmpty(streetNumber) && string.IsNullOrEmpty(route))
                return null;

            if (string.IsNullOrEmpty(streetNumber))
                return route;

            if (string.IsNullOrEmpty(route))
                return streetNumber;

            return $"{streetNumber} {route}";
        }

        /// <summary>
        /// Gets the state/province from address components
        /// </summary>
        /// <param name="useShortName">If true, returns short name (e.g., "NY"), otherwise long name (e.g., "New York")</param>
        /// <returns>State/province or null if not found</returns>
        public string GetState(bool useShortName = false)
        {
            if (AddressComponent == null)
                return null;

            var component = GetAddressComponentByType("administrative_area_level_1");
            return component == null ? null : (useShortName ? component.ShortName : component.LongName);
        }

        /// <summary>
        /// Gets the postal code from address components
        /// </summary>
        /// <returns>Postal code or null if not found</returns>
        public string GetPostalCode()
        {
            if (AddressComponent == null)
                return null;

            return GetAddressComponentByType("postal_code")?.LongName;
        }

        /// <summary>
        /// Gets the city/locality from address components
        /// </summary>
        /// <param name="useShortName">If true, returns short name, otherwise long name</param>
        /// <returns>City/locality or null if not found</returns>
        public string GetCity(bool useShortName = false)
        {
            if (AddressComponent == null)
                return null;

            // Try locality first, then sublocality
            var component = GetAddressComponentByType("locality") 
                          ?? GetAddressComponentByType("sublocality");
            
            return component == null ? null : (useShortName ? component.ShortName : component.LongName);
        }

        /// <summary>
        /// Gets the country from address components
        /// </summary>
        /// <param name="useShortName">If true, returns short name (e.g., "US"), otherwise long name (e.g., "United States")</param>
        /// <returns>Country or null if not found</returns>
        public string GetCountry(bool useShortName = false)
        {
            if (AddressComponent == null)
                return null;

            var component = GetAddressComponentByType("country");
            return component == null ? null : (useShortName ? component.ShortName : component.LongName);
        }

        /// <summary>
        /// Gets a complete address breakdown from address components
        /// </summary>
        /// <returns>Address breakdown object</returns>
        public AddressBreakdown GetAddressBreakdown()
        {
            if (AddressComponent == null)
                return new AddressBreakdown();

            return new AddressBreakdown
            {
                StreetAddress = GetStreetAddress(),
                City = GetCity(),
                State = GetState(),
                PostalCode = GetPostalCode(),
                Country = GetCountry()
            };
        }

        /// <summary>
        /// Helper method to find an address component by type
        /// </summary>
        /// <param name="type">The type to search for</param>
        /// <returns>Address component or null if not found</returns>
        private GoogleMapsApi.Entities.Geocoding.Response.AddressComponent GetAddressComponentByType(string type)
        {
            return AddressComponent?.FirstOrDefault(ac => ac.Types?.Contains(type) == true);
        }

        #endregion
    }

    /// <summary>
    /// Represents a complete address breakdown with individual components
    /// </summary>
    public class AddressBreakdown
    {
        /// <summary>
        /// Street address (street number + route)
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// City/locality
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State/province
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Returns a formatted address string
        /// </summary>
        /// <returns>Formatted address</returns>
        public override string ToString()
        {
            var parts = new[] { StreetAddress, City, State, PostalCode, Country }
                .Where(part => !string.IsNullOrEmpty(part))
                .ToArray();

            return string.Join(", ", parts);
        }
    }

    public enum PriceLevel
    {
        Free = 0,
        Inexpensive = 1,
        Moderate = 2,
        Expensive = 3,
        VeryExpensive = 4,
    }
}
