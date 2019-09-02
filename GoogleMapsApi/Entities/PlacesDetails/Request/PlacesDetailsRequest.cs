using System;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesDetails.Request
{
    public class PlacesDetailsRequest : MapsBaseRequest
    {
        protected internal override string BaseUrl => "maps.googleapis.com/maps/api/place/details/";

        public string PlaceId { get; set; } // required

        public string Language { get; set; } // optional

        protected override bool IsSsl
        {
            get => true;
            set => throw new NotSupportedException("This operation is not supported, PlacesRequest must use SSL");
        }
        
        /// <summary>
        /// A random string which identifies an autocomplete session for billing purposes.
        /// If this parameter is omitted from an autocomplete request, the request is billed independently.
        /// </summary>
        public string SessionToken { get; set; }

        protected override QueryStringParametersList GetQueryStringParameters()
        {
            if (string.IsNullOrWhiteSpace(PlaceId))
                throw new ArgumentException("PlaceId must be provided.");

            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey must be provided");

            QueryStringParametersList parameters = base.GetQueryStringParameters();

            parameters.Add("placeid", PlaceId);

            if (!string.IsNullOrWhiteSpace(Language)) parameters.Add("language", Language);
            
            if (!string.IsNullOrWhiteSpace(SessionToken))
                parameters.Add("sessiontoken", SessionToken);
            
            return parameters;
        }
    }
}