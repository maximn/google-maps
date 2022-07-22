using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNearBy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SnapToRoad.Response
{
	[DataContract]
	public class Result
	{
		/// <summary>
		/// name contains the human-readable name for the returned result. For establishment results, this is usually the canonicalized business name.
		/// </summary>
		/// 
		[DataMember(Name = "location")]
		public Location Location { get; set; }

		[DataMember(Name= "originalIndex")]
		public int OriginalIndex { get; set;}

		[DataMember(Name="placeId")]
		public string PlaceId { get; set; }

	}
}
