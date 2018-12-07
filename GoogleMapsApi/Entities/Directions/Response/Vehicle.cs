using System;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Directions.Response
{
	[DataContract(Name = "vehicle")]
	public class Vehicle
    {
        /// <summary>
        /// Contains the name of the vehicle on this line. eg. "Subway."
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Contains the type of vehicle that runs on this line.
        /// </summary>
        [DataMember(Name = "type")]
        public string VehicleTypeString
        {
            get => VehicleType.ToString();
            set => VehicleType = Enum.TryParse(value, out VehicleType vehicleType) ? vehicleType : VehicleType.OTHER;
        }

        /// <summary>
		/// Contains the type of vehicle that runs on this line.
		/// </summary>
		public VehicleType VehicleType { get; set; }

		/// <summary>
		/// Contains the URL for an icon associated with this vehicle type.
		/// </summary>
		[DataMember(Name = "icon")]
		public string Icon { get; set; }

        /// <summary>
        /// Contains the URL for a localized icon associated with this vehicle type.
        /// </summary>
        [DataMember(Name = "local_icon")]
        public string LocalIcon { get; set; }
    }
}