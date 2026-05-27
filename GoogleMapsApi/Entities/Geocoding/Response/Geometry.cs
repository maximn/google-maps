using GoogleMapsApi.Engine.JsonConverters;
using GoogleMapsApi.Entities.Common;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Geocoding.Response
{
	[DataContract]
	public class Geometry
	{
		/// <summary>
		/// location contains the geocoded latitude,longitude value. For normal address lookups, this field is typically the most important.
		/// </summary>
		[JsonPropertyName("location")]
		public Location Location { get; set; } = null!;

		/// <summary>
		/// location_type stores additional data about the specified location. 
		/// </summary>
		[JsonConverter(typeof(EnumMemberJsonConverter<GeocodeLocationType>))]
		public GeocodeLocationType LocationType { get; set; }

		/// <summary>
		/// viewport contains the recommended viewport for displaying the returned result, specified as two latitude,longitude values defining the southwest and northeast corner of the viewport bounding box. Generally the viewport is used to frame a result when displaying it to a user.
		/// </summary>
		[JsonPropertyName("viewport")]
		public FramedLocation ViewPort { get; set; } = null!;

		/// <summary>
		/// bounds (optionally returned) stores the bounding box which can fully contain the returned result. Note that these bounds may not match the recommended viewport. (For example, San Francisco includes the Farallon islands, which are technically part of the city, but probably should not be returned in the viewport.)
		/// </summary>
		[JsonPropertyName("bounds")]
		public FramedLocation Bounds { get; set; } = null!;


		[JsonPropertyName("location_type")]
		internal string LocationTypeStr
		{
			get
			{
				return LocationType.ToString();
			}
			set
			{
				LocationType = (GeocodeLocationType)Enum.Parse(typeof(GeocodeLocationType), value);
			}
		}
	}
}
