namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{
    using System;
    using System.Text.Json.Serialization;

    using GoogleMapsApi.Entities.Directions.Response;
    using GoogleMapsApi.Engine.JsonConverters;

    public class Element
    {
        /// <summary>
        /// "status" See Status Codes for a list of possible status codes.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumMemberJsonConverter<DistanceMatrixElementStatusCodes>))]
        public DistanceMatrixElementStatusCodes Status { get; set; }

        /// <summary>
		///  distance: The total distance of this route, expressed in meters (value) and as text
		/// </summary>
		[JsonPropertyName("distance")]
        public Distance Distance { get; set; } = null!;

        /// <summary>
        /// duration: The length of time it takes to travel this route
        /// </summary>
        [JsonPropertyName("duration")]
        public Duration Duration { get; set; } = null!;

        /// <summary>
		/// duration_in_traffic The length of time it takes to travel this route, based on current and historical traffic conditions. 
		/// See the traffic_model request parameter for the options you can use to request that the returned value is optimistic, pessimistic, or a best-guess estimate.
		/// </summary>
		[JsonPropertyName("duration_in_traffic")]
        public Duration? DurationInTraffic { get; set; }



    }
}
