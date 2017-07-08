[![Build Status](https://travis-ci.org/maximn/google-maps.svg?branch=master)](https://travis-ci.org/maximn/google-maps)
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

NEW! .NET core support with netstandard 1.5+ and net45+
NEW! .NET async support for webservice calls

This Library is well documented and easy to use. Check out our unit tests for working examples

Code sample -
``` C#
using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;

//Static class use (Directions) (Can be made from static/instance class)
DirectionsRequest directionsRequest = new DirectionsRequest()
{
    Origin = "NYC, 5th and 39",
    Destination = "Philladephia, Chesnut and Wallnut",
};

DirectionsResponse directions = await GoogleMaps.Directions.QueryAsync(directionsRequest);
Console.WriteLine(directions);

//Instance class use (Geocode)  (Can be made from static/instance class)
GeocodingRequest geocodeRequest = new GeocodingRequest()
{
    Address = "new york city",
};
var geocodingEngine = GoogleMaps.Geocode;
GeocodingResponse geocode = await geocodingEngine.QueryAsync(geocodeRequest);
Console.WriteLine(geocode);

// Static maps API - get static map of with the path of the directions request
StaticMapsEngine staticMapGenerator = new StaticMapsEngine();

//Path from previos directions request
IEnumerable<Step> steps = directions.Routes.First().Legs.First().Steps;
// All start locations
IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
// also the end location of the last step
path.Add(steps.Last().EndLocation);

string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(new Location(40.38742, -74.55366), 9, new ImageSize(800, 400))
{
    Pathes = new List<GoogleMapsApi.StaticMaps.Entities.Path>(){ new GoogleMapsApi.StaticMaps.Entities.Path()
    {
            Style = new PathStyle()
            {
                    Color = "red"
            },
            Locations = path
    }}
});
Console.WriteLine("Map with path: " + url);
```
# .net core

Note: This library supports the following bolded runtimes

| .NET Standard                             | 1.0   | 1.1	| 1.2	|1.3	|1.4	| **1.5**	| **1.6**	| **2.0**   |
| .NET Core                                 | **1.0**   | **1.0**	| **1.0**	| **1.0**	| **1.0**	| **1.0**	| **1.0**	| **2.0**   |
| .NET Framework (with tooling 1.0)         | **4.5**   | **4.5**	| **4.5.1**	| **4.6**	| **4.6.1**	| **4.6.2**	|	    |      |
| .NET Framework (with tooling 2.0 preview) | **4.5**   | **4.5**	| **4.5.1**	| **4.6**	| **4.6.1**	| **4.6.1**	| **4.6.1**	| **4.6.1** |
| Mono	                                    | **4.6**   | **4.6**	| **4.6**	| **4.6**	| **4.6**	| **4.6**	| **4.6**	| **vNext** |
| Xamarin.iOS                               | **10.0**  | **10.0**	| **10.0**	| **10.0**	| **10.0**	| **10.0**	| **10.0**	|vNext |
| Xamarin.Android                           | **7.0**   | **7.0**	| **7.0**	| **7.0**	| **7.0**	| **7.0**	| **7.0**	|vNext |
| Universal Windows Platform                | 10.0  | 10.0	| 10.0	|10.0	|10.0	| **vNext**	| **vNext**	| **vNext** |
| Windows                                   | 8.0   | 8.0	| 8.1	|		|		|       |       |      |
| Windows Phone	                            | 8.1   | 8.1	| 8.1	|		|		|       |       |      |
| Windows Phone Silverlight	                | 8.0   |       |       |       |       |       |       |      |

## netcore version differences from net45

1. A few APIs around number of parallel connections are not supported (because they use service endpoint manager. See here:  https://stackoverflow.com/questions/36398474/servicepointmanager-defaultconnectionlimit-in-net-core)
2. AuthenticationException is not supported on .net core, and instead an HttpRequestException will be given allowing users to respond to the http codes directly as they require. This is more flexible for the end user.