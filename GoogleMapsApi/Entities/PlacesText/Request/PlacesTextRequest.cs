using System;
using System.Globalization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesText.Request
{
    /// <summary>
    /// Request for the Google Places Text Search API, which returns places matching a free-form text query,
    /// optionally biased by location and radius.
    /// </summary>
    [Obsolete("The legacy Places API is frozen. Use the Places API (New) — e.g. IGoogleMapsClient.PlacesSearchText and the GoogleMapsApi.Entities.PlacesNew namespace.")]
    public class PlacesTextRequest : MapsBaseRequest
    {
        protected internal override string BaseUrl
        {
            get { return "maps.googleapis.com/maps/api/place/textsearch/"; }
        }

        public string Query { get; set; } = null!; // required

        public Location? Location { get; set; } // optional
        public double? Radius { get; set; } // optional
        public string? Language { get; set; } // optional
        public string? Types { get; set; } // optional
        public string? PageToken { get; set; } // optional

        public override bool IsSSL
        {
            get { return true; }
            set { throw new NotSupportedException("This operation is not supported, PlacesRequest must use SSL"); }
        }

        protected override QueryStringParametersList GetQueryStringParameters()
        {
            if (string.IsNullOrWhiteSpace(Query))
                throw new ArgumentException("Query must be provided.");

            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey must be provided");

            QueryStringParametersList parameters = base.GetQueryStringParameters();

            parameters.Add("query", Query);
            

            if (Location != null) parameters.Add("location", Location.ToString());
            if (Radius != null) parameters.Add("radius", Radius.Value.ToString(CultureInfo.InvariantCulture));
            if (!string.IsNullOrWhiteSpace(Language)) parameters.Add("language", Language);
            if (!string.IsNullOrWhiteSpace(Types)) parameters.Add("types", Types);
            if (!string.IsNullOrWhiteSpace(PageToken)) parameters.Add("pagetoken", PageToken);

            return parameters;
        }
    }
}
