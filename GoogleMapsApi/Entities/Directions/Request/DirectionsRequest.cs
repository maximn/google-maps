using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Directions.Request
{
	public class DirectionsRequest : MapsBaseRequest
	{
		/// <summary>
		/// origin (required) — The address or textual latitude/longitude value from which you wish to calculate directions. *
		/// </summary>
		public string Origin { get; set; } //Required
		/// <summary>
		/// destination (required) — The address or textual latitude/longitude value from which you wish to calculate directions.*
		/// </summary>
		public string Destination { get; set; } //Required

		/// <summary>
		/// waypoints (optional) specifies an array of waypoints. Waypoints alter a route by routing it through the specified location(s). A waypoint is specified as either a latitude/longitude coordinate or as an address which will be geocoded. (For more information on waypoints, see Using Waypoints in Routes below.)
		/// </summary>
		public string[] Waypoints { get; set; }

		/// <summary>
		/// alternatives (optional), if set to true, specifies that the Directions service may provide more than one route alternative in the response. Note that providing route alternatives may increase the response time from the server.
		/// </summary>
		public bool Alternatives { get; set; }

		/// <summary>
		/// avoid (optional) indicates that the calculated route(s) should avoid the indicated features. Currently, this parameter supports the following two arguments:
		//tolls indicates that the calculated route should avoid toll roads/bridges.
		//highways indicates that the calculated route should avoid highways.
		//(For more information see Route Restrictions below.)
		/// </summary>
		public AvoidWay Avoid { get; set; }

		/// <summary>
		/// language (optional) — The language in which to return results. See the supported list of domain languages. 
		/// Note that we often update supported languages so this list may not be exhaustive. 
		/// If language is not supplied, the Directions service will attempt to use the native language of the browser wherever possible. 
		/// You may also explicitly bias the results by using localized domains of http://map.google.com. 
		/// See Region Biasing for more information.
		/// </summary>
		public string Language { get; set; }


		/// <summary>
		/// (optional, defaults to driving) — specifies what mode of transport to use when calculating directions. Valid values are specified in Travel Modes.
		/// </summary>
		public TravelMode TravelMode { get; set; }
	}
}
