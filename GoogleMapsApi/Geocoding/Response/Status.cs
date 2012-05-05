using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Geocoding.Response
{
    /// <summary>
    /// The "status" field within the Geocoding response object contains the status of the request, and may contain debugging information to help you track down why Geocoding is not working. The "status" field may contain the following values:
    /// </summary>
    [DataContract]
    public enum Status
    {
        [EnumMember]
        OK, // indicates that no errors occurred; the address was successfully parsed and at least one geocode was returned.
        [EnumMember]
        ZERO_RESULTS, // indicates that the geocode was successful but returned no results. This may occur if the geocode was passed a non-existent address or a latlng in a remote location.
        [EnumMember]
        OVER_QUERY_LIMIT, // indicates that you are over your quota.
        [EnumMember]
        REQUEST_DENIED, // indicates that your request was denied, generally because of lack of a sensor parameter.
        [EnumMember]
        INVALID_REQUEST// generally indicates that the query (address or latlng) is missing.
    }
}
