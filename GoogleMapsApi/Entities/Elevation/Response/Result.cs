﻿using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Elevation.Response
{
	[DataContract]
	public class Result
	{
		/// <summary>
		/// A location element (containing lat and lng elements) of the position for which elevation data is being computed. Note that for path requests, the set of location elements will contain the sampled points along the path.
		/// </summary>
		[DataMember(Name = "location")]
		public Location Location { get; set; }

		/// <summary>
		/// An elevation element indicating the elevation of the location in meters.
		/// </summary>
		[DataMember(Name = "elevation")]
		public double Elevation { get; set; }

		/// <summary>
		/// The value indicating the maximum distance between data points from which the elevation was interpolated, in meters.
		/// This property will be missing if the resolution is not known. 
		/// Note that elevation data becomes more coarse (larger resolution values) when multiple points are passed.
		/// To obtain the most accurate elevation value for a point, it should be queried independently.
		/// </summary>
		[DataMember(Name = "resolution")]
		public double Resolution { get; set; }
	}
}
