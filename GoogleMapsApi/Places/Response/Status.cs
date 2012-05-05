using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Places.Response
{
    [DataContract]
    public enum Status
    {
        [EnumMember]
        OK, // indicates that no errors occurred; the place was successfully detected and at least one result was returned.
        [EnumMember]
        ZERO_RESULTS, // indicates that the search was successful but returned no results. This may occur if the search was passed a latlng in a remote location.
        [EnumMember]
        OVER_QUERY_LIMIT, // indicates that you are over your quota.
        [EnumMember]
        REQUEST_DENIED, // indicates that your request was denied, generally because of lack of a sensor parameter.
        [EnumMember]
        INVALID_REQUEST // generally indicates that the query parameter (location or radius) is missing.
    }
}
