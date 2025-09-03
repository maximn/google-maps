namespace GoogleMapsApi.Entities.PlaceAutocomplete.Response
{
	public enum Status
	{
		OK, // indicates that no errors occurred; the place was successfully detected and at least one result was returned.
		ZERO_RESULTS, // indicates that the search was successful but returned no results. This may occur if the search was passed a latlng in a remote location.
		OVER_QUERY_LIMIT, // indicates that you are over your quota.
		REQUEST_DENIED, // indicates that your request was denied.
		INVALID_REQUEST // generally indicates that the input parameter is missing.
	}
}
