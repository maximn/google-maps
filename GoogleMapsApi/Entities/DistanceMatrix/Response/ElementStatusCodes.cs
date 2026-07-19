namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{

	public enum DistanceMatrixElementStatusCodes
	{
		OK,                       // indicates the response contains a valid result.
		NOT_FOUND,                // indicates that the origin and/or destination of this pairing could not be geocoded.
		ZERO_RESULTS,             // indicates no route could be found between the origin and destination.
		MAX_ROUTE_LENGTH_EXCEEDED // indicates the requested route is too long and cannot be processed.
	}
}
