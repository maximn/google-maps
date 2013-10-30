using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Places.Response
{
	[DataContract]
	public class Result
	{
		/// <summary>
		/// name contains the human-readable name for the returned result. For establishment results, this is usually the canonicalized business name.
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "rating")]
		public double Rating { get; set; }

		[DataMember(Name = "icon")]
		public string Icon { get; set; }

		[DataMember(Name = "id")]
		public string ID { get; set; }

		[DataMember(Name = "reference")]
		public string Reference { get; set; }	

		[DataMember(Name = "vicinity")]
		public string Vicinity { get; set; }

		[DataMember(Name = "types")]
		public string[] Types { get; set; }

		[DataMember( Name = "geometry" )]
		public Geometry Geometry { get; set; }
	}
}
