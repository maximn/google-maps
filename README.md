[![NuGet Downloads](https://img.shields.io/nuget/dt/GoogleMapsApi.svg)](https://www.nuget.org/packages/GoogleMapsApi/)
[![NuGet Version](https://img.shields.io/nuget/v/GoogleMapsApi.svg)](https://www.nuget.org/packages/GoogleMapsApi/)
[![Build Status](https://github.com/maximn/google-maps/actions/workflows/dotnet.yml/badge.svg)](https://github.com/maximn/google-maps/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/maximn/google-maps/actions/workflows/codeql.yml/badge.svg)](https://github.com/maximn/google-maps/actions/workflows/codeql.yml)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/maximn/google-maps/badge)](https://scorecard.dev/viewer/?uri=github.com/maximn/google-maps)
[![License: BSD-2-Clause](https://img.shields.io/badge/License-BSD_2--Clause-blue.svg)](LICENSE.md)
[![.NET](https://img.shields.io/badge/.NET-net10.0%20%7C%20net8.0%20%7C%20netstandard2.0%20%7C%20net481%20%7C%20net462-512BD4)](https://dotnet.microsoft.com/)

Release history: see [CHANGELOG.md](CHANGELOG.md).

# GoogleMapsApi

A friendly, strongly-typed .NET wrapper for the Google Maps Web Services APIs — Geocoding, Routes, Directions, Distance Matrix, Elevation, Time Zone, Places, Address Validation, and Static Maps. Multi-framework (net10.0, net8.0, netstandard2.0, net481, net462), async-first, and battle-tested with **2M+ downloads** on NuGet.

## Supported APIs

| API | Description |
| --- | --- |
| [Geocoding](https://developers.google.com/maps/documentation/geocoding) | Convert between addresses and geographic coordinates |
| [Routes](https://developers.google.com/maps/documentation/routes) | Modern route planning — real-time traffic, eco-routing, toll calc, two-wheeled vehicles (replaces Directions) |
| [Directions](https://developers.google.com/maps/documentation/directions) | Legacy route planning between two points with multiple travel modes |
| [Distance Matrix](https://developers.google.com/maps/documentation/distance-matrix) | Travel time and distance between multiple origins/destinations |
| [Elevation](https://developers.google.com/maps/documentation/elevation) | Elevation data for individual locations or paths |
| [Time Zone](https://developers.google.com/maps/documentation/timezone) | Time zone information for any coordinate |
| [Places (New)](https://developers.google.com/maps/documentation/places/web-service/op-overview) | Modern Places API — Text Search, Nearby Search, Place Details, Autocomplete, Place Photos (replaces legacy Places) |
| [Places](https://developers.google.com/maps/documentation/places/web-service) | _Deprecated (frozen by Google)_ — legacy Find / Nearby / Text search, Place Details, Autocomplete. Use Places (New) instead |
| [Address Validation](https://developers.google.com/maps/documentation/address-validation) | Validate a postal address with component-level confirmation; USPS CASS for US/PR |
| [Static Maps](https://developers.google.com/maps/documentation/maps-static) | Generate URLs for static map images with markers, paths, and styles |

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

Looking for runnable examples? See [`samples/`](samples/) — console, ASP.NET Core minimal API, and Blazor Server.

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

For more configuration options and detailed guides, see the [wiki](https://github.com/maximn/google-maps/wiki). Full API reference is published at [maximn.github.io/google-maps](https://maximn.github.io/google-maps/).

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

### Routes API (modern replacement for Directions)

The Routes API is Google's modern replacement for the Directions API — it supports real-time traffic, eco-routing, toll calculation, two-wheeled vehicles, and route alternatives. Unlike Directions, it requires a [field mask](https://developers.google.com/maps/documentation/routes/choose_fields) to constrain the response. A sensible default is pre-populated; tighten it to reduce response size and cost.

``` C#
using GoogleMapsApi;
using GoogleMapsApi.Entities.Routes.Request;

var request = new RoutesRequest
{
    ApiKey = "your-google-maps-api-key",
    Origin = Waypoint.FromAddress("San Francisco, CA"),
    Destination = Waypoint.FromAddress("Mountain View, CA"),
    TravelMode = RoutesTravelMode.Drive,
    RoutingPreference = RoutingPreference.TrafficAware,
    // FieldMask defaults to a Directions-equivalent shape; override to slim the response.
};

var response = await GoogleMaps.Routes.QueryAsync(request);
var route = response.Routes![0];
Console.WriteLine($"{route.DistanceMeters} m, {route.DurationSeconds} s");
```

### Instance-based client (`IHttpClientFactory`-friendly)

In addition to the static `GoogleMaps` facade, you can use the instance-based `GoogleMapsClient` that accepts an injected `HttpClient`. This is the recommended pattern for ASP.NET Core, minimal APIs, and worker services — it plays nicely with `IHttpClientFactory`, per-instance event handlers, and an ambient API key that is auto-filled into requests when not set explicitly.

``` C#
// Register once at startup
services.AddHttpClient<IGoogleMapsClient, GoogleMapsClient>();
services.AddSingleton(new GoogleMapsClientOptions { ApiKey = "your-google-maps-api-key" });

// Inject and use
public class GeocodingService(IGoogleMapsClient maps)
{
    public Task<GeocodingResponse> LookupAsync(string address)
        => maps.Geocode.QueryAsync(new GeocodingRequest { Address = address });
}
```

Without DI:

``` C#
using var http = new HttpClient();
var maps = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "your-key" });

var result = await maps.Directions.QueryAsync(new DirectionsRequest { Origin = "NYC", Destination = "DC" });
```

Per-instance events (no global state):

``` C#
maps.Geocode.OnUriCreated += uri => uri;          // inspect/rewrite outgoing URI
maps.Geocode.OnRawResponseReceived += bytes => { }; // tap raw JSON
```

### Synchronous Usage

Synchronous calls are also supported via `Query` (use `QueryAsync` whenever possible):

``` C#
DirectionsResponse directions = GoogleMaps.Directions.Query(directionsRequest);
Console.WriteLine(directions);
```

---

*If this library saved you time, please ⭐ the repo — it helps others find it.*
