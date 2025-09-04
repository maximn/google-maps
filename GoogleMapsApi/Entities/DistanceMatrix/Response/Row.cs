namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Row
    {
        /// <summary>
		/// element[] The information about each origin-destination pairing is returned in an element entry
		/// </summary>
		[JsonPropertyName("elements")]
        public IEnumerable<Element> Elements { get; set; }
    }
}
