[![NuGet Downloads](https://img.shields.io/nuget/dt/GoogleMapsApi.svg)](https://www.nuget.org/packages/GoogleMapsApi/)
[![NuGet Version](https://img.shields.io/nuget/v/GoogleMapsApi.svg)](https://www.nuget.org/packages/GoogleMapsApi/)
[![Build Status](https://github.com/maximn/google-maps/actions/workflows/dotnet.yml/badge.svg)](https://github.com/maximn/google-maps/actions/workflows/dotnet.yml)
[![License: BSD-2-Clause](https://img.shields.io/badge/License-BSD_2--Clause-blue.svg)](LICENSE.md)
[![.NET](https://img.shields.io/badge/.NET-net10.0%20%7C%20net8.0%20%7C%20netstandard2.0%20%7C%20net481%20%7C%20net462-512BD4)](https://dotnet.microsoft.com/)

# GoogleMapsApi

A friendly, strongly-typed .NET wrapper for the Google Maps Web Services APIs — Geocoding, Directions, Distance Matrix, Elevation, Time Zone, Places, and Static Maps. Multi-framework (net10.0, net8.0, netstandard2.0, net481, net462), async-first, and battle-tested with **2M+ downloads** on NuGet.

## Supported APIs

| API | Description |
| --- | --- |
| Geocoding | Convert between addresses and geographic coordinates |
| Directions | Route planning between two points with multiple travel modes |
| Distance Matrix | Travel time and distance between multiple origins/destinations |
| Elevation | Elevation data for individual locations or paths |
| Time Zone | Time zone information for any coordinate |
| Places | Find / Nearby / Text search, Place Details, Autocomplete |
| Static Maps | Generate URLs for static map images with markers, paths, and styles |

## Why this vs Google's official SDKs

Google's official .NET packages (e.g. `Google.Maps.Routing.V2`, `Google.Maps.Places.V1`) are auto-generated from gRPC service definitions — they're verbose, split across many packages, and feel like protobuf instead of .NET. **GoogleMapsApi** is a single, idiomatic NuGet package: one install, async-first, multi-target (modern .NET through legacy .NET Framework), with hand-crafted request/response types that read like normal C#.

# Installation

Install via NuGet Package Manager:
```
Install-Package GoogleMapsApi
```

Or via .NET CLI:
```
dotnet add package GoogleMapsApi
```

# Quickstart

## API Key Configuration

You can configure your Google Maps API key in several ways:

```csharp
// Option 1: Set API key per request
DirectionsRequest directionsRequest = new DirectionsRequest()
{
    Origin = "NYC, 5th and 39",
    Destination = "Philadelphia, Chestnut and Walnut",
    ApiKey = "your-google-maps-api-key"
};

// Option 2: Set globally via app.config/appsettings.json (see wiki for details)
```

For more configuration options and detailed guides, see the [wiki](https://github.com/maximn/google-maps/wiki).

## Code Examples

### Basic Usage (async-first)

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
    Destination = "Philadelphia, Chestnut and Walnut",
};

// Async call (recommended)
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

### Synchronous Usage

Synchronous calls are also supported via `Query` (use `QueryAsync` whenever possible):

``` C#
DirectionsResponse directions = GoogleMaps.Directions.Query(directionsRequest);
Console.WriteLine(directions);
```

---

*If this library saved you time, please ⭐ the repo — it helps others find it.*
