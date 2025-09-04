namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    public enum Status
    {
        /// <summary>
        /// Indicates that no errors occurred; the place was successfully detected and at least one result was returned.
        /// </summary>
        OK,

        /// <summary>
        /// Indicates a server-side error; trying again may be successful.
        /// </summary>
        UNKNOWN_ERROR,

        /// <summary>
        /// Indicates that the search was successful but returned no results. This may occur if the search was passed a latlng in a remote location.
        /// </summary>
        ZERO_RESULTS,

        /// <summary>
        /// Indicates that you are over your quota.
        /// </summary>
        OVER_QUERY_LIMIT,

        /// <summary>
        /// Indicates that your request was denied.
        /// </summary>
        REQUEST_DENIED,

        /// <summary>
        /// Generally indicates that the query parameter (location or radius) is missing.
        /// </summary>
        INVALID_REQUEST,

        /// <summary>
        /// Indicates that the referenced location was not found in the Places database.
        /// </summary>
        NOT_FOUND,
    }
}
