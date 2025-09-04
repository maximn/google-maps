using System;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.TimeZone.Response
{
    public class TimeZoneResponse : IResponseFor<TimeZoneRequest>
    {        
        /// <summary>
        /// "status" contains metadata on the request.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumMemberJsonConverter<Status>))]
        public Status Status { get; set; }

        /// <summary>
        /// DstOffset: the offset for daylight-savings time in seconds. This will be zero if the time zone is not in Daylight Savings Time during the specified timestamp.
        /// </summary>
        [JsonPropertyName("dstOffset")]
        public double DstOffSet { get; set; }

        [Obsolete("Use DstOffSet instead.")]
        public double OffSet { get { return DstOffSet; }  }

        /// <summary>
        /// RawOffset: the offset from UTC (in seconds) for the given location. This does not take into effect daylight savings.
        /// </summary>
        [JsonPropertyName("rawOffset")]
        public double RawOffSet { get; set; }
        
        /// <summary>
        /// TimeZoneId: a string containing the ID of the time zone, such as "America/Los_Angeles" or "Australia/Sydney".
        /// </summary>
        [JsonPropertyName("timeZoneId")]
        public string TimeZoneId { get; set; }
        
        /// <summary>
        /// TimeZoneName: a string containing the long form name of the time zone. This field will be localized if the language parameter is set. eg. "Pacific Daylight Time" or "Australian.
        /// </summary>
        [JsonPropertyName("timeZoneName")]
        public string TimeZoneName { get; set; }
    }
}
 
 

