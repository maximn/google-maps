namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{
    using System;
    using System.Runtime.Serialization;

    using GoogleMapsApi.Entities.Directions.Response;

    [DataContract(Name = "element")]
    public class Element
    {
        [DataMember(Name = "status")]
        public string StatusStr
        {
            get => Status.ToString();
            set => Status = (DistanceMatrixElementStatusCodes)Enum.Parse(typeof(DistanceMatrixElementStatusCodes), value);
        }

        /// <summary>
        /// "status" See Status Codes for a list of possible status codes.
        /// </summary>
        public DistanceMatrixElementStatusCodes Status { get; set; }

        /// <summary>
		///  distance: The total distance of this route, expressed in meters (value) and as text
		/// </summary>
		[DataMember(Name = "distance")]
        public Distance Distance { get; set; }

        /// <summary>
        /// duration: The length of time it takes to travel this route
        /// </summary>
        [DataMember(Name = "duration")]
        public Duration Duration { get; set; }

        /// <summary>
		/// duration_in_traffic The length of time it takes to travel this route, based on current and historical traffic conditions. 
		/// See the traffic_model request parameter for the options you can use to request that the returned value is optimistic, pessimistic, or a best-guess estimate.
		/// </summary>
		[DataMember(Name = "duration_in_traffic")]
        public Duration DurationInTraffic { get; set; }



    }
}
