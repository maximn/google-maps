namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{
	using System.Runtime.Serialization;

	[DataContract]
	public enum DistanceMatrixElementStatusCodes
	{
		[EnumMember]
		OK,                       // indicates the response contains a valid result.
		[EnumMember]
		NOT_FOUND,                // indicates that the origin and/or destination of this pairing could not be geocoded.
		[EnumMember]
		ZERO_RESULTS,             // indicates no route could be found between the origin and destination.
		[EnumMember]
		MAX_ROUTE_LENGTH_EXCEEDED // indicates the requested route is too long and cannot be processed.
	}
}