using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.StaticMaps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Roads.SnapToRoad;
using GoogleMapsApi.Entities.Roads.SnapToRoad.Request;
using GoogleMapsApi.Entities.Geocoding.Request;

namespace GoogleMapsApi.StaticMaps
{
	public class RouteMapsEngine
	{


		public string GenerateRouteMapURLSnap(RouteMapRequest request)
		{
			GeocodingRequest grA = new GeocodingRequest()
			{
				ApiKey = request.ApiKey,
				Address = request.Origin,

			};
			GeocodingRequest grB = new GeocodingRequest()
			{
				ApiKey = request.ApiKey,
				Address = request.Destination,

			};
			Location locA = GoogleMaps.Geocode.QueryAsync(grA).Result.Results.FirstOrDefault().Geometry.Location;
			Location locB = GoogleMaps.Geocode.QueryAsync(grB).Result.Results.FirstOrDefault().Geometry.Location;

			const int MAX_POINTS = 98;

			var path = (IList<ILocationString>)LocationInterpolator.GetList(
						//new GoogleMapsApi.Entities.Common.Location(50.467192, 30.473800),
						//new GoogleMapsApi.Entities.Common.Location(50.461517, 30.482252)
						locA,
						locB,
						MAX_POINTS


						);
			SnapToRoadRequest snapToRoadRequest = new SnapToRoadRequest()
			{
				ApiKey = request.ApiKey,
				Path = path,
				


			};
			var snapToRoadResult = GoogleMaps.SnapToRoads.QueryAsync(snapToRoadRequest);
			var points = snapToRoadResult.Result.snappedPoints;
			List<GoogleMapsApi.Entities.Roads.Location> locs = new List<GoogleMapsApi.Entities.Roads.Location>();
			foreach(var p in points)
			{
				var loc = new GoogleMapsApi.Entities.Common.Location(p.Location.Latitude,p.Location.Longitude);
				locs.Add(p.Location);
			}
			StaticMapRequest staticMapRequest = new StaticMapRequest(request.Center, 12, request.Size)
			{
				ImageFormat = request.ImageFormat,
				Center = request.Center,
				Scale = request.Scale,
				Language = request.Language,
				IsSSL = request.IsSSL,
				ApiKey = request.ApiKey,
				Zoom = request.Zoom,
				Pathes = new List<Path>()
				{
					new Path()
					{
						Locations = new List<ILocationString>(locs),
						Style = new PathStyle()
						{
							Color = "blue",
							Weight = 10

						}

					}
				},


			};
			return new StaticMapsEngine().GenerateStaticMapURL(staticMapRequest);
		}

		public string GenerateRouteMapURL(RouteMapRequest request)
		{
			DirectionsRequest directionsRequest = new DirectionsRequest()
			{
				ApiKey = request.ApiKey, //TODO: refactor
				ArrivalTime = request.ArrivalTime,
				Origin = request.Origin,
				Destination = request.Destination,
				IsSSL = request.IsSSL,
				TravelMode = request.TravelMode,
				Language = request.Language,
				Region = request.Region
			};

			var directionsRequestResult = GoogleMaps.Directions.QueryAsync(directionsRequest);
			var locs = directionsRequestResult.Result.Routes.FirstOrDefault().OverviewPath.Points;

			if(request.CalculateZoom)
			{
				int distance = directionsRequestResult.Result.Routes.FirstOrDefault().Legs.FirstOrDefault().Distance.Value;
				int km = distance / 1000;
				int zMin = 0;
				int zMax = 21;

				//km = ( 40000/2 ^ zl ) * 2
				// let zoom = getBaseLog(2, 40000 / (km / 2))

				request.Zoom = (int)Math.Log((int)(40000 / (km / 2)), (int)2)-2;
			}


			StaticMapRequest staticMapRequest = new StaticMapRequest(request.Center, 12, request.Size)
			{
				ImageFormat = request.ImageFormat,
				Center = request.Center,
				Scale = request.Scale,
				Language = request.Language,
				IsSSL = request.IsSSL,
				ApiKey = request.ApiKey,
				Zoom = request.Zoom,
				Pathes = new List<Path>()
				{
					new Path()
					{
						Locations = new List<ILocationString>(locs),
						Style = new PathStyle()
						{
							Color = "blue",
							Weight = 10
							
						}

					}
				},


			};
			return new StaticMapsEngine().GenerateStaticMapURL(staticMapRequest);
		}
	}
}
