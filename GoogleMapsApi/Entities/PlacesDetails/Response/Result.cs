using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections.Generic;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    [DataContract]
    public class Result
    {
        /// <summary>
        /// An array containing the separate components applicable to this address.
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
        public BusinessStatus BusinessStatus { get; set; } 
            
        /// <summary>
        /// Specifies if the business supports curbside pickup.
        /// </summary>
        [DataMember(Name = "curbside_pickup")]
        public bool CurbsidePickup { get; set; }
        
        /// <summary>
        /// Contains the hours of operation for the next seven days (including today).
        /// </summary>
        [DataMember(Name = "current_opening_hours")]
        public OpeningHours CurrentOpeningHours { get; set; }
        
        /// <summary>
        /// Specifies if the business supports delivery.
        /// </summary>
        [DataMember(Name = "delivery")]
        public bool Delivery { get; set; }

        /// <summary>
        /// Specifies if the business supports indoor or outdoor seating options.
        /// </summary>
        [DataMember(Name = "dine_in")]
        public bool DineIn { get; set; }
        
        /// <summary>
        /// Contains a summary of the place.
        /// </summary>
        [DataMember(Name = "editorial_summary")]
        public PlaceEditorialSummary PlaceEditorialSummary { get; set; }
        
        /// <summary>
        /// A string containing the human-readable address of this place. 
        /// </summary>
        [DataMember(Name = "formatted_address")]
        public string FormattedAddress { get; set; }

        /// <summary>
        /// Contains the place's phone number in its local format.
        /// </summary>
        [DataMember(Name = "formatted_phone_number")]
        public string FormattedPhoneNumber { get; set; }

        /// <summary>
        /// Contains the location and viewport for the location.
        /// </summary>
        [DataMember(Name = "geometry")]
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Contains the URL of a suggested icon which may be displayed to the user when indicating this result on a map.
        /// </summary>
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

        /// <summary>
        /// Contains the place's phone number in international format.
        /// </summary>
        [DataMember(Name = "international_phone_number")]
        public string InternationalPhoneNumber { get; set; }

        /// <summary>
        /// Contains the human-readable name for the returned result.
        /// For establishment results, this is usually the canonicalized business name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Contains the regular hours of operation.
        /// </summary>
        [DataMember(Name = "opening_hours")]
        public OpeningHours OpeningHours { get; set; }
        
        /// <depreciated /> 
        [DataMember(Name = "permanently_closed")]
        [Obsolete("Use business_status to get the operational status of businesses.")]
        public bool PermanentlyClosed { get; set; }

        /// <summary>
        /// An array of photo objects, each containing a reference to an image.
        /// A request may return up to ten photos.
        /// </summary>
        [DataMember(Name = "photos")]
        public IEnumerable<Photo> Photos { get; set; }

        /// <summary>
        /// A textual identifier that uniquely identifies a place.
        /// </summary>
        [DataMember(Name = "place_id")]
        public string PlaceId { get; set; }

        /// <summary>
        /// An encoded location reference, derived from latitude and longitude coordinates, that represents an area:
        /// 1/8000th of a degree by 1/8000th of a degree (about 14m x 14m at the equator) or smaller.
        /// Plus codes can be used as a replacement for street addresses in places where they do not exist
        /// (where buildings are not numbered or streets are not named).
        /// </summary>
        [DataMember(Name = "plus_code")]
        public PlusCode PlusCode { get; set; }

        /// <summary>
        /// The price level of the place, on a scale of 0 to 4.
        /// The exact amount indicated by a specific value will vary from region to region.
        /// </summary>
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

        /// <summary>
        /// Contains the place's rating, from 1.0 to 5.0, based on aggregated user reviews.
        /// </summary>
        [DataMember(Name = "rating")]
        public double Rating { get; set; }
        
        /// <depreciated />
        [DataMember(Name = "reference")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
        public string Reference { get; set; }

        /// <summary>
        /// A JSON array of up to five reviews. By default, the reviews are sorted in order of relevance.
        /// </summary>
        [DataMember(Name = "reviews")]
        public IEnumerable<Review> Reviews { get; set; }

        /// <summary>
        /// Contains an array of entries for the next seven days including information about secondary hours of a business.
        /// Secondary hours are different from a business's main hours.
        /// For example, a restaurant can specify drive through hours or delivery hours as its secondary hours.
        /// </summary>
        [DataMember(Name = "secondary_opening_hours")]
        public IEnumerable<OpeningHours> SecondaryOpeningHours { get; set; }

        /// <summary>
        /// Specifies if the place serves beer.
        /// </summary>
        [DataMember(Name = "serves_beer")]
        public bool ServesBeer { get; set; }
        
        /// <summary>
        /// Specifies if the place serves breakfast.
        /// </summary>
        [DataMember(Name = "serves_breakfast")]
        public bool ServesBreakfast { get; set; }
        
        /// <summary>
        /// Specifies if the place serves brunch.
        /// </summary>
        [DataMember(Name = "serves_brunch")]
        public bool ServesBrunch { get; set; }
        
        /// <summary>
        /// Specifies if the place serves dinner.
        /// </summary>
        [DataMember(Name = "serves_dinner")]
        public bool ServesDinner { get; set; }
        
        /// <summary>
        /// Specifies if the place serves lunch.
        /// </summary>
        [DataMember(Name = "serves_lunch")]
        public bool ServesLunch { get; set; }
        
        /// <summary>
        /// Specifies if the place serves vegetarian food.
        /// </summary>
        [DataMember(Name = "serves_vegetarian_food")]
        public bool ServesVegetarianFood { get; set; }
        
        /// <summary>
        /// Specifies if the place serves wine.
        /// </summary>
        [DataMember(Name = "serves_wine")]
        public bool ServesWine { get; set; }
        
        /// <summary>
        /// Specifies if the business supports takeout.
        /// </summary>
        [DataMember(Name = "takeout")]
        public bool Takeout { get; set; }

        /// <summary>
        /// Contains an array of feature types describing the given result.
        /// </summary>
        [DataMember(Name = "types")]
        public IEnumerable<string> Types { get; set; }

        /// <summary>
        /// Contains the URL of the official Google page for this place.
        /// This will be the Google-owned page that contains the best available information about the place.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// The total number of reviews, with or without text, for this place.
        /// </summary>
        [DataMember(Name = "user_ratings_total")]
        public int UserRatingsTotal { get; set; }
        
        /// <summary>
        /// Contains the number of minutes this place’s current timezone is offset from UTC.
        /// For example, for places in Sydney, Australia during daylight saving time this would be 660 (+11 hours from UTC),
        /// and for places in California outside of daylight saving time this would be -480 (-8 hours from UTC).
        /// </summary>
        [DataMember(Name = "utc_offset")]
        public int UtcOffset { get; set; }

        /// <summary>
        /// For establishment (types:["establishment", ...]) results only, the vicinity field contains a simplified address for the place, including the street name, street number, and locality, but not the province/state, postal code, or country.
        /// For all other results, the vicinity field contains the name of the narrowest political (types:["political", ...]) feature that is present in the address of the result.
        /// </summary>
        [DataMember(Name = "vicinity")]
        public string Vicinity { get; set; }

        /// <summary>
        /// The authoritative website for this place, such as a business' homepage.
        /// </summary>
        [DataMember(Name = "website")]
        public string Website { get; set; }

        /// <summary>
        /// Specifies if the place has an entrance that is wheelchair-accessible.
        /// </summary>
        [DataMember(Name = "wheelchair_accessible_entrance")]
        public bool WheelchairAccessibleEntrance { get; set; }
    }
}
