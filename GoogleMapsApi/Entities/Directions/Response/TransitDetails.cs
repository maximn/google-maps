using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Directions.Response
{
	public class TransitDetails
	{
		/// <summary>
		/// Contains information about the stop/station for this part of the trip
		/// </summary>
		[JsonPropertyName("arrival_stop")]
		public Stop? ArrivalStop { get; set; }

		/// <summary>
		/// Contains information about the stop/station for this part of the trip.
		/// </summary>
		[JsonPropertyName("departure_stop")]
		public Stop? DepartureStop { get; set; }

		/// <summary>
		/// Contain the arrival times for this leg of the journey
		/// </summary>
		[JsonPropertyName("arrival_time")]
		public Duration? ArrivalTime { get; set; }

		/// <summary>
		/// Contain the departure times for this leg of the journey
		/// </summary>
		[JsonPropertyName("departure_time")]
		public Duration? DepartureTime { get; set; }

		/// <summary>
		/// Specifies the direction in which to travel on this line, as it is marked on the vehicle or at the departure stop. This will often be the terminus station.
		/// </summary>
		[JsonPropertyName("headsign")]
		public string? Headsign { get; set; }

		/// <summary>
		/// Specifies the expected number of seconds between departures from the same stop at this time. For example, with a headway value of 600, you would expect a ten minute wait if you should miss your bus.
		/// </summary>
		[JsonPropertyName("headway")]
		public int Headway { get; set; }

		/// <summary>
		/// Contains the number of stops in this step, counting the arrival stop, but not the departure stop. For example, if your directions involve leaving from Stop A, passing through stops B and C, and arriving at stop D, num_stops will return 3.
		/// </summary>
		[JsonPropertyName("num_stops")]
		public int NumberOfStops { get; set; }

		/// <summary>
		/// Contains information about the transit line used in this step.
		/// </summary>
		[JsonPropertyName("line")]
		public Line? Lines { get; set; }
	}
}