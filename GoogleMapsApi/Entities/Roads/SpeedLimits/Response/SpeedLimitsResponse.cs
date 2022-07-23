using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;
using GoogleMapsApi.Entities.Roads.SnapToRoad.Request;
using GoogleMapsApi.Entities.Roads.SnapToRoad.Response;
using GoogleMapsApi.Entities.Roads.SpeedLimits.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SpeedLimits.Response
{
	public class SpeedLimitsResponse : IResponseFor<SpeedLimitsRequest>
	{
		public Status Status { get; set; }

		[DataMember(Name = "status")]
		internal string StatusStr
		{
			get
			{
				return Status.ToString();
			}
			set
			{
				Status = (Status)Enum.Parse(typeof(Status), value);
			}
		}

		/// <summary>
		/// If there is a next page of results, the token will be supplied by Google. Token should be set on the next PlacesRequest object to get the next page of results from Google.
		/// If null is returned, there is no next page of results.
		/// </summary>
		[DataMember(Name = "next_page_token")]
		public string NextPage
		{
			get;
			set;
		}

		/// <summary>
		/// "results" contains an array of places, with information about the place. See Place Search Results for information about these results. The Places API returns up to 20 establishment results. Additionally, political results may be returned which serve to identify the area of the request.
		/// </summary>
		[DataMember(Name = "speedLimits")]
		public IEnumerable<SpeedLimitResult> Limits { get; set; }
		[DataMember(Name = "snappedPoints")]
		public IEnumerable<SnappedPointResult> SnappedPoints { get; set; }
		[DataMember(Name ="warningMessage")]
		public string WarningMessage { get; set; }
	}
}
