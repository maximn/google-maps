using GoogleMapsApi.Entities.Common;
using System;
using System.Globalization;

namespace GoogleMapsApi.Entities.PlacesFind.Request
{
    /// <summary>
    /// A Find Place request takes a text input, and returns a place. The text input can be any kind of Places data, for example, a name, address, or phone number.
    /// </summary>
    public class PlacesFindRequest : MapsBaseRequest
    {
        protected internal override string BaseUrl
        {
            get
            {
                return "maps.googleapis.com/maps/api/place/findplacefromtext/";
            }
        }

        /// <summary>
        /// Required. The text input specifying which place to search for (for example, a name, address, or phone number).
        /// </summary>
        public string Input { get; set; } = null!;

        /// <summary>
        /// Required. The type of input. This can be one of either textquery or phonenumber. Phone numbers must be in international format (prefixed by a plus sign ("+"), followed by the country code, then the phone number itself). See E.164 ITU recommendation for more information.
        /// </summary>
        public InputType? InputType { get; set; }

        /// <summary>
        /// Optional. The language code, indicating in which language the results should be returned, if possible. Searches are also biased to the selected language; results in the selected language may be given a higher ranking. See the list of supported languages and their codes. Note that we often update supported languages so this list may not be exhaustive.
        /// </summary>
        public string? Language { get; set; }

        /// <summary>
        /// Optional. The fields specifying the types of place data to return, separated by a comma. If you omit the fields parameter from a Find Place request, only the place_id for the result will be returned. See docs for more information on 
        /// </summary>
        public string? Fields { get; set; }

        /// <summary>
        /// Prefer results in a specified area, by specifying either a radius plus lat/lng, or two lat/lng pairs representing the points of a rectangle. If this parameter is not specified, the API uses IP address biasing by default. See docs for more information on how to format value.
        /// </summary>
        public string? LocationBias { get; set; }

        public override bool IsSSL
        {
            get
            {
                return true;
            }
            set
            {
                throw new NotSupportedException("This operation is not supported, PlacesFindRequest must use SSL");
            }
        }

        protected override QueryStringParametersList GetQueryStringParameters()
        {
            if (Input == null)
                throw new ArgumentException("Input must be provided.");
            if (InputType == null)
                throw new ArgumentException("InputType must be provided.");
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey must be provided");

            var parameters = base.GetQueryStringParameters();
            parameters.Add("key", ApiKey);
            parameters.Add("input", Input);
            parameters.Add("inputtype", InputType == Request.InputType.PhoneNumber ? "phonenumber" : "textquery");

            // optional parameters
            if (!string.IsNullOrWhiteSpace(Language))
                parameters.Add("language", Language);
            if (!string.IsNullOrWhiteSpace(Fields))
                parameters.Add("fields", Fields);
            if (!string.IsNullOrWhiteSpace(LocationBias))
                parameters.Add("locationbias", LocationBias);

            return parameters;
        }
    }
}
