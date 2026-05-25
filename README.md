[![NuGet Downloads](https://img.shields.io/nuget/dt/GoogleMapsApi.svg)](https://www.nuget.org/packages/GoogleMapsApi/)
[![NuGet Version](https://img.shields.io/nuget/v/GoogleMapsApi.svg)](https://www.nuget.org/packages/GoogleMapsApi/)
[![Build Status](https://github.com/maximn/google-maps/actions/workflows/dotnet.yml/badge.svg)](https://github.com/maximn/google-maps/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/maximn/google-maps/actions/workflows/codeql.yml/badge.svg)](https://github.com/maximn/google-maps/actions/workflows/codeql.yml)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/maximn/google-maps/badge)](https://scorecard.dev/viewer/?uri=github.com/maximn/google-maps)
[![License: BSD-2-Clause](https://img.shields.io/badge/License-BSD_2--Clause-blue.svg)](LICENSE.md)
[![.NET](https://img.shields.io/badge/.NET-net10.0%20%7C%20net8.0%20%7C%20netstandard2.0-512BD4)](https://dotnet.microsoft.com/)

Release history: see [CHANGELOG.md](CHANGELOG.md).

# GoogleMapsApi

A friendly, strongly-typed .NET wrapper for the Google Maps Web Services APIs — Geocoding, Directions, Distance Matrix, Elevation, Time Zone, Places, and Static Maps. Multi-framework (net10.0, net8.0, netstandard2.0), async-first, and battle-tested with **2M+ downloads** on NuGet.

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

Google's official .NET packages (e.g. `Google.Maps.Routing.V2`, `Google.Maps.Places.V1`) are auto-generated from gRPC service definitions — they're verbose, split across many packages, and feel like protobuf instead of .NET. **GoogleMapsApi** is a single, idiomatic NuGet package: one install, async-first, multi-target (net10.0, net8.0, plus netstandard2.0 for legacy .NET Framework consumers), with hand-crafted request/response types that read like normal C#.

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

You can configure your Google Maps API key in two ways:

```csharp
// Option 1: ambient default on the client (auto-filled into every request
// that doesn't set its own ApiKey)
IGoogleMapsClient maps = new GoogleMapsClient(httpClient, new GoogleMapsClientOptions
{
    ApiKey = "your-google-maps-api-key",
});

// Option 2: per-request override (wins over the ambient default)
var request = new DirectionsRequest
{
    Origin = "NYC, 5th and 39",
    Destination = "Philadelphia, Chestnut and Walnut",
    ApiKey = "another-google-maps-api-key",
};
```

Full API reference is published at [maximn.github.io/google-maps](https://maximn.github.io/google-maps/).

## Code Examples

### Basic usage

Construct a single `GoogleMapsClient` per process (typically via `IHttpClientFactory`) and reuse it across calls:

``` C#
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

using var http = new HttpClient();
IGoogleMapsClient maps = new GoogleMapsClient(http, new GoogleMapsClientOptions
{
    ApiKey = "your-google-maps-api-key",
});

DirectionsResponse directions = await maps.Directions.QueryAsync(new DirectionsRequest
{
    Origin = "NYC, 5th and 39",
    Destination = "Philadelphia, Chestnut and Walnut",
});

GeocodingResponse geocode = await maps.Geocode.QueryAsync(new GeocodingRequest
{
    Address = "new york city",
});
```

### Dependency injection (ASP.NET Core / minimal APIs / workers)

``` C#
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IGoogleMapsClient>(sp => new GoogleMapsClient(
    sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(GoogleMapsClient)),
    new GoogleMapsClientOptions { ApiKey = builder.Configuration["GoogleMaps:ApiKey"] }));

// Inject and use
public class GeocodingService(IGoogleMapsClient maps)
{
    public Task<GeocodingResponse> LookupAsync(string address)
        => maps.Geocode.QueryAsync(new GeocodingRequest { Address = address });
}
```

> Coming in 2.1: `services.AddGoogleMaps(o => o.ApiKey = ...)` collapses the two lines above into one.

### Per-instance events

``` C#
maps.Geocode.OnUriCreated += uri => uri;          // inspect/rewrite outgoing URI
maps.Geocode.OnRawResponseReceived += bytes => { }; // tap raw JSON
```

### Static maps URL generation

``` C#
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;

var staticMapGenerator = new StaticMapsEngine();

IEnumerable<Step> steps = directions.Routes.First().Legs.First().Steps;
IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
path.Add(steps.Last().EndLocation);

string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(
    new Location(40.38742, -74.55366), 9, new ImageSize(800, 400))
{
    Pathes = new List<GoogleMapsApi.StaticMaps.Entities.Path>
    {
        new GoogleMapsApi.StaticMaps.Entities.Path
        {
            Style = new PathStyle { Color = "red" },
            Locations = path,
        }
    }
});
```

### Synchronous calls

Sync overloads exist on `IEngineFacade<>` and block the calling thread via `GetAwaiter().GetResult()`. Prefer `QueryAsync` whenever possible — sync calls can deadlock on single-threaded SynchronizationContexts (classic ASP.NET, WinForms, WPF UI threads).

``` C#
DirectionsResponse directions = maps.Directions.Query(directionsRequest);
```

---

*If this library saved you time, please ⭐ the repo — it helps others find it.*
