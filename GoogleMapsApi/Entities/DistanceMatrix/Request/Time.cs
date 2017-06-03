
namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{
	using System;
	using System.Globalization;

	using GoogleMapsApi.Engine;

	public class Time
	{
		public DateTime Value { get; set; }

		public bool IsNow { get; set; }

		public Time()
		{
			IsNow = true;
		}

		public override string ToString()
		{
			return IsNow ? "now" : UnixTimeConverter.DateTimeToUnixTimestamp(Value).ToString(CultureInfo.InvariantCulture);
		}
	}
}