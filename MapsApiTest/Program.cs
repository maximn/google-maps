using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleMapsApi;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Elevation.Response;
using System.Reflection;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using GoogleMapsApi.StaticMaps.Enums;

namespace MapsApiTest
{
	class Program
	{
		static void Main(string[] args)
		{
			//Static class use (Directions)
			DirectionsRequest directionsRequest = new DirectionsRequest()
			{
				Origin = "NYC, 5th and 39",
				Destination = "Philladephia, Chesnut and Wallnut",
			};

			DirectionsResponse directions = MapsAPI.GetDirections(directionsRequest);

			Console.WriteLine(directions);


			//Instance class use (Geocode)
			GeocodingRequest geocodeRequest = new GeocodingRequest()
			{
				Address = "new york city",
			};

			GeocodingEngine geocodingEngine = new GeocodingEngine();

			GeocodingResponse geocode = geocodingEngine.GetGeocode(geocodeRequest);

			Console.WriteLine(geocode);


			// Static maps API - get static map of with the path of the directions request
			StaticMapsEngine staticMapGenerator = new StaticMapsEngine();

			//Path from previos directions request
			IEnumerable<Step> steps = directions.Routes.First().Legs.First().Steps;
			// All start locations
			IList<ILocation> path = steps.Select(step => step.StartLocation).ToList<ILocation>();
			// also the end location of the last step
			path.Add(steps.Last().EndLocation);

			string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(new Location(40.38742, -74.55366), 9, new ImageSize(800, 400))
			{
				Pathes = new List<Path>(){ new Path()
	{
		Style = new PathStyle()
		{
			Color = "red"
		},
		Locations = path
	}}


			});

			Console.WriteLine("Map with path: " + url);



			//Instance class - Async! (Elevation)
			ElevationRequest elevationRequest = new ElevationRequest()
			{
				Locations = new Location[] { new Location(54, 78) },
			};

			ElevationEngine elevationEngine = new ElevationEngine();

			elevationEngine.BeginGetElevation(elevationRequest,
																				ar =>
																				{
																					ElevationResponse elevation = elevationEngine.EndGetElevation(ar);
																					Console.WriteLine(elevation);
																				},
																				null);

			Console.WriteLine("Finised! (But wait .. async elevation request should get response soon)");



			Console.ReadKey();
		}
	}
}
