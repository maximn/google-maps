[![Build status](https://ci.appveyor.com/api/projects/status/gkr2dk9uw35cg7ex?svg=true)](https://ci.appveyor.com/project/maximn/google-maps)
[![NuGet Status](https://img.shields.io/nuget/v/GoogleMapsApi.svg)](https://www.nuget.org/packages/GoogleMapsApi/)

google-maps
===========

Google Maps Web Services API wrapper for .NET

For Quickstart and more info read the wiki pages (https://github.com/maximn/google-maps/wiki)

The web page - http://maximn.github.com/google-maps

NuGet page - https://www.nuget.org/packages/GoogleMapsApi/


**Check out my blog at http://maxondev.com**

# Quickstart

This library wraps Google maps API.

You can easily query Google maps for Geocoding, Directions, Elevation, and Places.

NEW! Now you can easily show the results on a Static Google Map!

This Library is well documented and easy to use.

Code sample -
``` C#
	//Static class use (Directions) (Can be made from static/instance class)
	DirectionsRequest directionsRequest = new DirectionsRequest()
	{
			Origin = "NYC, 5th and 39",
			Destination = "Philladephia, Chesnut and Wallnut",
	};

	DirectionsResponse directions = MapsAPI.GetDirections(directionsRequest);

	Console.WriteLine(directions);


	//Instance class use (Geocode)  (Can be made from static/instance class)
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
	
	
```
