using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.StaticMaps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.StaticMaps
{
	public class RouteMapsEngine
	{


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

			StaticMapRequest staticMapRequest = new StaticMapRequest(request.Center, 12, request.Size)
			{
				ImageFormat = request.ImageFormat,
				Center = request.Center,
				Scale = request.Scale,
				Language = request.Language,
				IsSSL = request.IsSSL,
				ApiKey = request.ApiKey,
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
