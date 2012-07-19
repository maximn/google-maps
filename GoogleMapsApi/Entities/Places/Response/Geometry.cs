using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Places.Response
{
	[DataContract]
	public class Geometry
	{
		/// <summary>
		/// location contains the geocoded latitude,longitude value. For normal address lookups, this field is typically the most important.
		/// </summary>
		[DataMember(Name = "location")]
		public Location Location { get; set; }
	}
}
