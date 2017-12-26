using GoogleMapsApi.Entities.Common;
using System;
using System.Globalization;

namespace GoogleMapsApi.Entities.PlaceAutocomplete.Request
{
    /// <summary>
    /// The Place Autocomplete service is a web service that returns place predictions in response to an HTTP request. The request specifies 
    /// a textual search string and optional geographic bounds. The service can be used to provide autocomplete functionality for text-based 
    /// geographic searches, by returning places such as businesses, addresses and points of interest as a user types.
    /// </summary>
    public class PlaceAutocompleteRequest : MapsBaseRequest
	{
		protected internal override string BaseUrl
		{
			get
			{
				return "maps.googleapis.com/maps/api/place/autocomplete/";
			}
		}

		/// <summary>
		/// Input (required) - the text string on which to search
		/// </summary>
		public string Input { get; set; }

		/// <summary>
		/// The position, in the input term, of the last character that the service uses to match predictions (optional).
		/// For example, if the input is 'Google' and the offset is 3, the service will match on 'Goo'. The string determined by the offset 
		/// is matched against the first word in the input term only. For example, if the input term is 'Google abc' and the offset is 3, 
		/// the service will attempt to match against 'Goo abc'. If no offset is supplied, the service will use the whole term. The offset 
		/// should generally be set to the position of the text caret.
		/// </summary>
		public int? Offset { get; set; }

		/// <summary>
		/// location (optional) — The textual latitude/longitude value from which you wish to retrieve place information.
		/// </summary>
		public Location Location { get; set; }

		/// <summary>
		/// Defines the distance (in meters) within which to return Place results (optional). The maximum allowed radius is 50 000 meters. 
		/// Note that radius must not be included if rankby=distance (described under Optional parameters below) is specified.
		/// </summary>
		public double? Radius { get; set; }

		/// <summary>
		/// The language code, indicating in which language the results should be returned, if possible. See the list of supported languages and their codes. 
		/// Note that we often update supported languages so this list may not be exhaustive.
		/// </summary>
		public string Language { get; set; }

        /// <summary>
        /// (optional) Restricts the results to Places matching the specified type . 
        /// See the list of supported types - https://developers.google.com/maps/documentation/places/supported_types
        /// 
        /// </summary>
        public string Type { get; set; }

		/// <summary>
		///  A grouping of places to which you would like to restrict your results (optional). Currently, you can use components to filter by country. 
		///  The country must be passed as a two character, ISO 3166-1 Alpha-2 compatible country code. For example: components=country:fr would 
		///  restrict your results to places within France.
		/// </summary>
		public string Components { get; set; }

		/// <summary>
		///  Returns only those places that are strictly within the region defined by location and radius. 
        	///  This is a restriction, rather than a bias, meaning that results outside this region will not be returned even if they match the user input
		/// </summary>
        	public bool StrinctBounds { get; set; }

		public override bool IsSSL
		{
			get
			{
				return true;
			}
			set
			{
				throw new NotSupportedException("This operation is not supported, PlaceAutocompleteRequest must use SSL");
			}
		}

		protected override QueryStringParametersList GetQueryStringParameters()
		{
			if (Input == null)
				throw new ArgumentException("Input term must be provided.");
			if (string.IsNullOrWhiteSpace(ApiKey))
				throw new ArgumentException("ApiKey must be provided");

			var parameters = base.GetQueryStringParameters();
			parameters.Add("input", Input);
			parameters.Add("key", ApiKey);

			if (Offset.HasValue)
				parameters.Add("offset", Offset.Value.ToString(CultureInfo.InvariantCulture));
			if (Location != null)
				parameters.Add("location", Location.ToString());
			if (Radius.HasValue)
				parameters.Add("radius", Radius.Value.ToString(CultureInfo.InvariantCulture));
			if (!string.IsNullOrWhiteSpace(Language))
				parameters.Add("language", Language);
			if (!string.IsNullOrWhiteSpace(Type))
				parameters.Add("type", Type);
			if (!string.IsNullOrWhiteSpace(Components))
				parameters.Add("components", Components);
			if (StrinctBounds)
                		parameters.Add("strictbounds", StrinctBounds.ToString());

			return parameters;
		}
	}
}
