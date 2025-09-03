using System;
using System.Text.Json.Serialization;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.Directions.Response
{
	public class Vehicle
    {
        /// <summary>
        /// Contains the name of the vehicle on this line. eg. "Subway."
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
		/// Contains the type of vehicle that runs on this line.
		/// </summary>
		[JsonPropertyName("type")]
		[JsonConverter(typeof(EnumMemberJsonConverter<VehicleType>))]
		public VehicleType VehicleType { get; set; }

		/// <summary>
		/// Contains the URL for an icon associated with this vehicle type.
		/// </summary>
		[JsonPropertyName("icon")]
		public string Icon { get; set; }

        /// <summary>
        /// Contains the URL for a localized icon associated with this vehicle type.
        /// </summary>
        [JsonPropertyName("local_icon")]
        public string LocalIcon { get; set; }
    }
}