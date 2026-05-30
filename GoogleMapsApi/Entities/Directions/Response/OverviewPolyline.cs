using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;


namespace GoogleMapsApi.Entities.Directions.Response
{
	/// <summary>
	/// Contains the encoded and decoded data returned in the overview_polyline field.
	/// </summary>
	public class OverviewPolyline
	{
		/// <summary>
		/// The encoded string containing the overview path points as they were received.
		/// </summary>
		[JsonPropertyName("points")]
		internal string? EncodedPoints { get; set; }

		private Lazy<IEnumerable<Location>> pointsLazy = null!;

		/// <summary>
		/// An array of Location objects representing the points in the overview path, decoded from the string contained in the EncodedPoints property.
		/// </summary>
		/// <exception cref="PointsDecodingException">Unexpectedly couldn't decode points</exception>
		public IEnumerable<Location> Points { get { return pointsLazy.Value; } }

		public OverviewPolyline()
		{
			InitLazyPoints();
		}

		// Initialize lazy points after deserialization
		private void InitLazyPoints()
		{
			pointsLazy = new Lazy<IEnumerable<Location>>(DecodePoints);
		}

		// Called after deserialization to initialize lazy points
		internal void OnDeserialized()
		{
			InitLazyPoints();
		}

		/// <exception cref="PointsDecodingException">Unexpectedly couldn't decode points</exception>
		private IEnumerable<Location> DecodePoints() => EncodedPolylineDecoder.Decode(EncodedPoints);

		/// <summary>
		/// The RAW data of points from Google
		/// </summary>
		/// <returns></returns>
		public string? GetRawPointsData()
		{
			return this.EncodedPoints;
		}
	}
}