using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Elevation.Response
{
    [DataContract]
    public enum Status
    {
        [EnumMember]
        OK, // indicating the API request was successful
        [EnumMember]
        INVALID_REQUEST, // indicating the API request was malformed
        [EnumMember]
        OVER_QUERY_LIMIT, // indicating the requestor has exceeded quota
        [EnumMember]
        REQUEST_DENIED, // indicating the API did not complete the request, likely because the requestor failed to include a valid sensor parameter
        [EnumMember]
        UNKNOWN_ERROR //indicating an unknown error
    }
}
