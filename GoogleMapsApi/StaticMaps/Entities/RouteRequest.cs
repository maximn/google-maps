using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.StaticMaps.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.StaticMaps.Entities
{
	class RouteMapRequest:MapsBaseRequest
	{
		// ------ FROM STATICMAPREQUEST ------

		/// <summary>
		/// (required if markers not present) defines the center of the map, equidistant from all edges of the map.
		/// This parameter takes a location as either a comma-separated {latitude,longitude} pair (e.g. "40.714728,-73.998672") or
		/// a string address (e.g. "city hall, new york, ny") identifying a unique location on the face of the earth.
		/// </summary>
		public ILocationString Center { get; set; }

		/// <summary>
		///  (required if markers not present) defines the zoom level of the map, which determines the magnification level of the map.
		/// This parameter takes a numerical value corresponding to the zoom level of the region desired.
		/// Maps on Google Maps have an integer "zoom level" which defines the resolution of the current view. Zoom levels between 0 (the lowest zoom level, in which the entire world can be seen on one map) to 21+ (down to individual buildings) are possible within the default roadmap maps view.
		/// Google Maps sets zoom level 0 to encompass the entire earth. Each succeeding zoom level doubles the precision in both horizontal and vertical dimensions. More information on how this is done is available in the Google Maps API documentation.
		/// Note: not all zoom levels appear at all locations on the earth. Zoom levels vary depending on location, as data in some parts of the globe is more granular than in other locations.
		/// If you send a request for a zoom level in which no map tiles exist, the Static Maps API will return a blank image instead.
		/// </summary>
		public int Zoom { get; set; }

		/// <summary>
		/// scale (optional) affects the number of pixels that are returned. scale=2 returns twice as many pixels as scale=1 while retaining the same coverage area and level of detail (i.e. the contents of the map don't change). This is useful when developing for high-resolution displays, or when generating a map for printing. The default value is 1. Accepted values are 2 and 4 (4 is only available to Google Maps APIs Premium Plan customers.) See Scale Values for more information.
		/// </summary>
		public int Scale { get; set; }

		/// <summary>
		/// (required) defines the rectangular dimensions of the map image.
		/// This parameter takes a string of the form valuexvalue where horizontal pixels are denoted first while vertical pixels are
		/// denoted second. For example, 500x400 defines a map 500 pixels wide by 400 pixels high.
		/// If you create a static map that is 100 pixels wide or smaller, the "Powered by Google" logo is automatically reduced in size.
		/// </summary>
		public ImageSize Size { get; set; }

		/// <summary>
		/// (optional) defines the format of the resulting image.
		/// By default, the Static Maps API creates PNG images.
		/// There are several possible formats including GIF, JPEG and PNG types.
		/// Which format you use depends on how you intend to present the image.
		/// JPEG typically provides greater compression, while GIF and PNG provide greater detail.
		/// </summary>
		public ImageFormat ImageFormat { get; set; }

		/// <summary>
		///  (optional) defines the type of map to construct.
		/// There are several possible maptype values, including roadmap, satellite, hybrid, and terrain.
		/// </summary>
		public MapType? MapType { get; set; }

		/// <summary>
		/// (optional) defines the language to use for display of labels on map tiles.
		/// Note that this parameter is only supported for some country tiles;
		/// if the specific language requested is not supported for the tile set, then the default language for that tileset will be used.
		/// </summary>
		public string Language { get; set; }

		/// <summary>
		/// (optional) define one or more markers to attach to the image at specified locations.
		/// This parameter takes a single marker definition with parameters separated by the pipe character (|).
		/// Multiple markers may be placed within the same markers parameter as long as they exhibit the same style;
		/// you may add additional markers of differing styles by adding additional markers parameters.
		/// Note that if you supply markers for a map, you do not need to specify the (normally required) center and zoom parameters.
		/// </summary>
		public IList<Marker> Markers { get; set; }

		/// <summary>
		///  (optional) defines a single path of two or more connected points to overlay on the image at specified locations.
		/// This parameter takes a string of point definitions separated by the pipe character (|).
		/// You may supply additional paths by adding additional path parameters.
		/// Note that if you supply a path for a map, you do not need to specify the (normally required) center and zoom parameters.
		/// </summary>
		public IList<Path> Pathes { get; set; }

		/// <summary>
		/// (optional) specifies one or more locations that should remain visible on the map,
		/// though no markers or other indicators will be displayed.
		/// Use this parameter to ensure that certain features or map locations are shown on the static map.
		/// </summary>
		public ILocationString Visible { get; set; }

		/// <summary>
		/// (optional) defines a custom style to alter the presentation of a specific feature (road, park, etc.) of the map.
		/// This parameter takes feature and element arguments identifying the features to select and a set of style operations to
		/// apply to that selection. You may supply multiple styles by adding additional style parameters.
		/// </summary>
		public MapStyle Style { get; set; }

		public bool IsSSL { get; set; }

		/// <summary>
		/// Add API key support for the static map. This parameter is required for all static map requests.
		/// </summary>
		public string ApiKey { get; set; }

		public RouteMapRequest(ILocationString center, int zoom, ImageSize imageSize)
		{
			Center = center;
			Zoom = zoom;
			Size = imageSize;
		}

		public RouteMapRequest(IList<Marker> markers, ImageSize imageSize)
		{
			Markers = markers;
			Size = imageSize;
		}

		// ------- FROM DIRECTIONSREQUEST ------- 

		/// <summary>
		/// origin (Required) - The address or textual latitude/longitude value from which you wish to calculate directions. 
		/// If you pass an address as a string, the Directions service will geocode the string and convert it to a latitude/longitude 
		/// coordinate to calculate directions. If you pass coordinates, ensure that no space exists between the latitude and longitude values.
		/// </summary>
		public string Origin { get; set; }

		/// <summary>
		/// destination (required) — The address or textual latitude/longitude value from which you wish to calculate directions. 
		/// If you pass an address as a string, the Directions service will geocode the string and convert it to a latitude/longitude 
		/// coordinate to calculate directions. If you pass coordinates, ensure that no space exists between the latitude and longitude values.
		/// </summary>
		public string Destination { get; set; }

		/// <summary>
		/// The time of departure.
		/// Required when TravelMode = Transit
		/// </summary>
		public DateTime DepartureTime { get; set; }

		/// <summary>
		/// The time of arrival.
		/// Required when TravelMode = Transit
		/// </summary>
		public DateTime ArrivalTime { get; set; }

		/// <summary>
		/// waypoints (optional) specifies an array of waypoints. Waypoints alter a route by routing it through the specified location(s). A waypoint is specified as either a latitude/longitude coordinate or as an address which will be geocoded. (For more information on waypoints, see Using Waypoints in Routes below.)
		/// </summary>
		public string[] Waypoints { get; set; }

		/// <summary>
		/// optimize the provided route by rearranging the waypoints in a more efficient order. (This optimization is an application of the Travelling Salesman Problem.)
		/// http://en.wikipedia.org/wiki/Travelling_salesman_problem
		/// </summary>
		public bool OptimizeWaypoints { get; set; }

		/// <summary>
		/// alternatives (optional), if set to true, specifies that the Directions service may provide more than one route alternative in the response. Note that providing route alternatives may increase the response time from the server.
		/// </summary>
		public bool Alternatives { get; set; }

		/// <summary>
		/// avoid (optional) indicates that the calculated route(s) should avoid the indicated features. Currently, this parameter supports the following two arguments:
		/// tolls indicates that the calculated route should avoid toll roads/bridges.
		/// highways indicates that the calculated route should avoid highways.
		/// (For more information see Route Restrictions below.)
		/// </summary>
		public AvoidWay Avoid { get; set; }

		/// <summary>
		/// (optional, defaults to driving) — specifies what mode of transport to use when calculating directions. Valid values are specified in Travel Modes.
		/// </summary>
		public TravelMode TravelMode { get; set; }

		/// <summary>
		/// This parameter takes a region code, specified as a IANA language region subtag (http://www.iana.org/assignments/language-subtag-registry/language-subtag-registry). 
		/// In most cases, these tags map directly to familiar ccTLD ("top-level domain") two-character values such as "uk" in "co.uk" for example.
		///  In some cases, the region tag also supports ISO-3166-1 codes, which sometimes differ from ccTLD values ("GB" for "Great Britain" for example).
		/// </summary>
		public string Region { get; set; }

		
	}
}
