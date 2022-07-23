using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNearBy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SpeedLimits.Response
{
	[DataContract]
	public class SpeedLimitResult
	{
		/// <summary>
		/// name contains the human-readable name for the returned result. For establishment results, this is usually the canonicalized business name.
		/// </summary>
		/// 
		[DataMember(Name = "speedLimit")]
		public int Limit { get; set; }

		[DataMember(Name= "units")]
		public string Unit { get; set;}

		[DataMember(Name="placeId")]
		public string PlaceId { get; set; }

	}
}
