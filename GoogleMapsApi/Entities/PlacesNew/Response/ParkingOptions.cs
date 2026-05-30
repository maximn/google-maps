using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Parking options available at a place.</summary>
    public sealed class ParkingOptions
    {
        /// <summary>Whether a free parking lot is available.</summary>
        [JsonPropertyName("freeParkingLot")]
        public bool? FreeParkingLot { get; set; }

        /// <summary>Whether a paid parking lot is available.</summary>
        [JsonPropertyName("paidParkingLot")]
        public bool? PaidParkingLot { get; set; }

        /// <summary>Whether free street parking is available.</summary>
        [JsonPropertyName("freeStreetParking")]
        public bool? FreeStreetParking { get; set; }

        /// <summary>Whether paid street parking is available.</summary>
        [JsonPropertyName("paidStreetParking")]
        public bool? PaidStreetParking { get; set; }

        /// <summary>Whether valet parking is available.</summary>
        [JsonPropertyName("valetParking")]
        public bool? ValetParking { get; set; }

        /// <summary>Whether free garage parking is available.</summary>
        [JsonPropertyName("freeGarageParking")]
        public bool? FreeGarageParking { get; set; }

        /// <summary>Whether paid garage parking is available.</summary>
        [JsonPropertyName("paidGarageParking")]
        public bool? PaidGarageParking { get; set; }
    }
}
