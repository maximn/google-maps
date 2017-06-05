namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{
    using System.Runtime.Serialization;

    [DataContract]
	public enum DistanceMatrixStatusCodes
	{
		[EnumMember]
		OK, // indicates the response contains a valid result.
		[EnumMember]
		INVALID_REQUEST, // indicates that the provided request was invalid.
        [EnumMember]
        MAX_ELEMENTS_EXCEEDED, // indicates that the product of origins and destinations exceeds the per-query limit.
        [EnumMember]
		OVER_QUERY_LIMIT, // indicates the service has received too many requests from your application within the allowed time period.
		[EnumMember]
		REQUEST_DENIED, // indicates that the service denied use of the directions service by your application.
		[EnumMember]
		UNKNOWN_ERROR, // indicates a directions request could not be processed due to a server error. The request may succeed if you try again
	}
}
