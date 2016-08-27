using System;
using System.Collections.Generic;
using System.Linq;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Geocoding.Request
{
	public class GeocodingRequest : SignableRequest
	{
		protected internal override string BaseUrl
		{
			get
			{
				return base.BaseUrl + "geocode/";
			}
		}

		/// <summary>
		/// address (required) — The address that you want to geocode.*
		/// </summary>
		public string Address { get; set; } //Required *or Location

		/// <summary>
		/// latlng (required) — The textual latitude/longitude value for which you wish to obtain the closest, human-readable address.*
		/// If you pass a latlng, the geocoder performs what is known as a reverse geocode. See Reverse Geocoding for more information.
		/// </summary>
		public Location Location { get; set; } //Required *or Address

		/// <summary>
		/// bounds (optional) — The bounding box of the viewport within which to bias geocode results more prominently. (For more information see Viewport Biasing below.)
		/// The bounds and region parameters will only influence, not fully restrict, results from the geocoder.
		/// </summary>
		public Location[] Bounds { get; set; }

		/// <summary>
		/// region (optional) — The region code, specified as a ccTLD ("top-level domain") two-character value. (For more information see Region Biasing below.)
		/// The bounds and region parameters will only influence, not fully restrict, results from the geocoder.
		/// </summary>
		public string Region { get; set; }

		/// <summary>
		/// language (optional) — The language in which to return results. See the supported list of domain languages. Note that we often update supported languages so this list may not be exhaustive. If language is not supplied, the geocoder will attempt to use the native language of the domain from which the request is sent wherever possible.
		/// </summary>
		public string Language { get; set; }

        #region Components
        /// <summary>
        /// A component filter for which you wish to obtain a geocode. The components filter will also be accepted as an optional parameter if an address is provided.
        /// See more info: https://developers.google.com/maps/documentation/geocoding/intro#ComponentFiltering
        /// </summary>
        public Dictionary<string, string> Components { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// matches long or short name of a route.
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        /// matches against both locality and sublocality types.
        /// </summary>
        public string Locality { get; set; }
        /// <summary>
        /// matches all the administrative_area levels.
        /// </summary>
        public string AdministrativeArea { get; set; }
        /// <summary>
        /// matches postal_code and postal_code_prefix.
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// matches a country name or a two letter ISO 3166-1 country code.
        /// </summary>
        public string Country { get; set; }

        private string BuildComponents()
        {
            if (!string.IsNullOrWhiteSpace(Route))
                Components[route] = Route;

            if (!string.IsNullOrWhiteSpace(Locality))
                Components[locality] = Locality;

            if (!string.IsNullOrWhiteSpace(AdministrativeArea))
                Components[administrative_area] = AdministrativeArea;

            if (!string.IsNullOrWhiteSpace(PostalCode))
                Components[postal_code] = PostalCode;

            if (!string.IsNullOrWhiteSpace(Country))
                Components[country] = Country;

            string components = "";
            foreach (var keyValue in Components)
            {
                components += $"{keyValue.Key}:{keyValue.Value}|";
            }
            return components;
        }
        public const string route = "route";
        public const string locality = "locality";
        public const string administrative_area = "administrative_area";
        public const string postal_code = "postal_code";
        public const string country = "country";
        #endregion

        protected override QueryStringParametersList GetQueryStringParameters()
		{
            string components = BuildComponents();
            bool hasComponents = !string.IsNullOrWhiteSpace(components);
            if (string.IsNullOrWhiteSpace(Address) && Location == null && !hasComponents)
				throw new ArgumentException("Address, Location OR Components is required");

			var parameters = base.GetQueryStringParameters();

			if (Location != null)
				parameters.Add(_latlng, Location.ToString());
			else
				parameters.Add(_address, Address);

			if (Bounds != null && Bounds.Any())
				parameters.Add(_bounds, string.Join("|", Bounds.AsEnumerable()));

			if (!string.IsNullOrWhiteSpace(Region))
				parameters.Add(_region, Region);

			if (!string.IsNullOrWhiteSpace(Language))
				parameters.Add(_language, Language);
            
            if (hasComponents)
                parameters.Add(_components, components);

            return parameters;
		}

        const string _latlng = "latlng";
        const string _address = "address";
        const string _bounds = "bounds";
        const string _region = "region";
        const string _language = "language";
        const string _components = "components";
    }
}
