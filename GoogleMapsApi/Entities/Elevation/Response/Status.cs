namespace GoogleMapsApi.Entities.Elevation.Response
{
	public enum Status
	{
		OK, // indicating the API request was successful
		INVALID_REQUEST, // indicating the API request was malformed
		OVER_QUERY_LIMIT, // indicating the requestor has exceeded quota
		REQUEST_DENIED, // indicating the API did not complete the request
		UNKNOWN_ERROR //indicating an unknown error
	}
}
