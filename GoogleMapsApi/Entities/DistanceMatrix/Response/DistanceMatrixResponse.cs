namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    using GoogleMapsApi.Entities.Common;
    using GoogleMapsApi.Entities.Directions.Response;
    using GoogleMapsApi.Entities.DistanceMatrix.Request;
    using GoogleMapsApi.Engine.JsonConverters;

    public class DistanceMatrixResponse: IResponseFor<DistanceMatrixRequest>
    {
        /// <summary>
        /// "status" contains metadata on the request. See Status Codes below.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumMemberJsonConverter<DistanceMatrixStatusCodes>))]
        public DistanceMatrixStatusCodes Status { get; set; }

        [JsonPropertyName("rows")]
        public IEnumerable<Row> Rows { get; set; } = null!;

        [JsonPropertyName("destination_addresses")]
        public IEnumerable<string> DestinationAddresses { get; set; } = null!;


        [JsonPropertyName("origin_addresses")]
        public IEnumerable<string> OriginAddresses { get; set; } = null!;
        
        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; } = null!;
    }
}
