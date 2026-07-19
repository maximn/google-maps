namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{

	public enum DistanceMatrixStatusCodes
	{
		OK, // indicates the response contains a valid result.
		NOT_FOUND,//indicates at least one of the locations specified in the request's origin, destination, or waypoints could not be geocoded.
		ZERO_RESULTS,// indicates no route could be found between the origin and destination.
		MAX_WAYPOINTS_EXCEEDED, // 
		MAX_ROUTE_LENGTH_EXCEEDED, // indicates the requested route is too long and cannot be processed.
		INVALID_REQUEST, // indicates that the provided request was invalid.
		MAX_ELEMENTS_EXCEEDED, // indicates that the product of origins and destinations exceeds the per-query limit.
		OVER_QUERY_LIMIT, // indicates the service has received too many requests from your application within the allowed time period.
		REQUEST_DENIED, // indicates that the service denied use of the directions service by your application.
		UNKNOWN_ERROR, // indicates a directions request could not be processed due to a server error. The request may succeed if you try again
	}
}
