﻿using System;
using System.Globalization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesDetails.Request
{
    public class PlacesDetailsRequest : MapsBaseRequest
    {
        protected internal override string BaseUrl
        {
            get { return "maps.googleapis.com/maps/api/place/details/"; }
        }

        public string ApiKey { get; set; } // required
        public string Reference { get; set; } // required

        public string Language { get; set; } // optional

        public override bool IsSSL
        {
            get { return true; }
            set { throw new NotSupportedException("This operation is not supported, PlacesRequest must use SSL"); }
        }

        protected override QueryStringParametersList GetQueryStringParameters()
        {
            if (string.IsNullOrWhiteSpace(Reference))
                throw new ArgumentException("Reference must be provided.");

            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey must be provided");

            QueryStringParametersList parameters = base.GetQueryStringParameters();

            parameters.Add("reference", Reference);
            parameters.Add("key", ApiKey);

            if (!string.IsNullOrWhiteSpace(Language)) parameters.Add("language", Language);
            
            return parameters;
        }
    }
}