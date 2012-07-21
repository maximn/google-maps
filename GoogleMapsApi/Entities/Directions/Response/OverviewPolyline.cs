using System.Collections.Generic;
using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Directions.Response
{
	/// <summary>
	/// Contains the encoded and decoded data returned in the overview_polyline field.
	/// </summary>
	[DataContract]
	public class OverviewPolyline
	{
		/// <summary>
		/// The encoded string containing the overview path points as they were received.
		/// </summary>
		[DataMember(Name = "points")]
		internal string EncodedPoints { get; set; }

		/// <summary>
		/// An array of Location objects representing the points in the overview path, decoded from the string contained in the EncodedPoints property.
		/// </summary>
		public Location[] Points { get; set; }

		// Adapted from http://jeffreysambells.com/2010/05/27/decoding-polylines-from-google-maps-direction-api-with-java
		[OnDeserialized]
		private void DecodePoints(StreamingContext context)
		{
			try
			{
				var poly = new List<Location>();
				int index = 0;
				int lat = 0;
				int lng = 0;

				while (index < EncodedPoints.Length)
				{
					int b, shift = 0, result = 0;
					do
					{
						b = EncodedPoints[index++] - 63;
						result |= (b & 0x1f) << shift;
						shift += 5;
					} while (b >= 0x20);
					int dlat = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
					lat += dlat;

					shift = 0;
					result = 0;
					do
					{
						b = EncodedPoints[index++] - 63;
						result |= (b & 0x1f) << shift;
						shift += 5;
					} while (b >= 0x20);
					int dlng = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
					lng += dlng;

					poly.Add(new Location((int)((lat / 1E5) * 1E6), (int)((lng / 1E5) * 1E6)));
				}

				Points = poly.ToArray();
			}
			catch
			{
				Points = null;
			}
		}

	}
}