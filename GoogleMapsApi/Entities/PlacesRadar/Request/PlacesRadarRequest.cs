using GoogleMapsApi.Entities.Common;
using System;
using System.Globalization;

namespace GoogleMapsApi.Entities.PlacesRadar.Request
{
    /// <summary>
    /// Find out how to replace it -
    /// https://cloud.google.com/blog/products/maps-platform/announcing-deprecation-of-place-add
    /// </summary>
    [Obsolete("Radar search is deprecated since June 30 2018", true)]
    public class PlacesRadarRequest : MapsBaseRequest
	{
		protected internal override string BaseUrl => "maps.googleapis.com/maps/api/place/radarsearch/";

        /// <summary>
        /// Required. The latitude/longitude around which to retrieve place information. This must be specified as latitude,longitude.
		/// </summary>
		public Location Location { get; set; } //Required

		/// <summary>
        /// Required. Defines the distance (in meters) within which to return place results. The maximum allowed radius is 50 000 meters.
		/// </summary>
		public double Radius { get; set; }

		/// <summary>
        /// Optional. A term to be matched against all content that Google has indexed for this place, including but not limited to name, type, and address, as well as customer reviews and other third-party content.
		/// </summary>
		public string Keyword { get; set; }

        /// <summary>
        /// Optional. Restricts results to only those places within the specified price level. Valid values are in the range from 0 (most affordable) to 4 (most expensive), inclusive. The exact amount indicated by a specific value will vary from region to region.
        /// </summary>
        public int? MinPrice { get; set; }

        /// <summary>
        /// Optional. Restricts results to only those places within the specified price level. Valid values are in the range from 0 (most affordable) to 4 (most expensive), inclusive. The exact amount indicated by a specific value will vary from region to region.
        /// </summary>
        public int? MaxPrice { get; set; }

		/// <summary>
        /// Optional. One or more terms to be matched against the names of places, separated by a space character. Results will be restricted to those containing the passed name values. Note that a place may have additional names associated with it, beyond its listed name. The API will try to match the passed name value against all of these names. As a result, places may be returned in the results whose listed names do not match the search term, but whose associated names do.
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Optional. Set to true to only return those places that are open for business at the time the query is sent. Places that do not specify opening hours in the Google Places database will not be returned if you include this parameter in your query.
        /// </summary>
        public bool OpenNow { get; set; }

		/// <summary>
		/// Optional. Restricts the results to Places matching the specified type. 
		/// See the list of supported types - https://developers.google.com/maps/documentation/places/supported_types
		/// </summary>
		public string Type { get; set; }

        protected override bool IsSsl
		{
			get => true;
            set => throw new NotSupportedException("This operation is not supported, PlacesRequest must use SSL");
        }

		protected override QueryStringParametersList GetQueryStringParameters()
		{
			if (Location == null)
				throw new ArgumentException("Location must be provided.");
			if ((Radius > 50000 || Radius < 1))
				throw new ArgumentException("Radius must be greater than or equal to 1 and less than or equal to 50000.");
			if (string.IsNullOrWhiteSpace(ApiKey))
				throw new ArgumentException("ApiKey must be provided");

			var parameters = base.GetQueryStringParameters();
			parameters.Add("location", Location.ToString());
			parameters.Add("key", ApiKey);
            parameters.Add("radius", Radius.ToString(CultureInfo.InvariantCulture));

			if (!string.IsNullOrWhiteSpace(Keyword))
                parameters.Add("keyword", Keyword);
			if (MinPrice.HasValue)
                parameters.Add("minprice", MinPrice.Value.ToString(CultureInfo.InvariantCulture));
            if (MaxPrice.HasValue)
                parameters.Add("maxprice", MaxPrice.Value.ToString(CultureInfo.InvariantCulture));
            if (!string.IsNullOrWhiteSpace(Name))
                parameters.Add("name", Name); 
            if (OpenNow)
                parameters.Add("opennow", "true"); 
            if (!string.IsNullOrWhiteSpace(Type))
				parameters.Add("type", Type);

			return parameters;
		}
	}
}
