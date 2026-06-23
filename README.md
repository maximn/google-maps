[![NuGet Downloads](https://img.shields.io/nuget/dt/GoogleMapsApi.svg)](https://www.nuget.org/packages/GoogleMapsApi/)
[![NuGet Version](https://img.shields.io/nuget/v/GoogleMapsApi.svg)](https://www.nuget.org/packages/GoogleMapsApi/)
[![Build Status](https://github.com/maximn/google-maps/actions/workflows/dotnet.yml/badge.svg)](https://github.com/maximn/google-maps/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/maximn/google-maps/actions/workflows/codeql.yml/badge.svg)](https://github.com/maximn/google-maps/actions/workflows/codeql.yml)
[![codecov](https://codecov.io/gh/maximn/google-maps/branch/master/graph/badge.svg)](https://codecov.io/gh/maximn/google-maps)
[![OpenSSF Scorecard](https://img.shields.io/ossf-scorecard/github.com/maximn/google-maps?label=openssf%20scorecard)](https://scorecard.dev/viewer/?uri=github.com/maximn/google-maps)
[![License: BSD-2-Clause](https://img.shields.io/badge/License-BSD_2--Clause-blue.svg)](LICENSE.md)
[![.NET](https://img.shields.io/badge/.NET-net10.0%20%7C%20net8.0%20%7C%20netstandard2.0-512BD4)](https://dotnet.microsoft.com/)

Release history: see [CHANGELOG.md](CHANGELOG.md).

[![Sponsor](https://readme.cash/i/xx0nyicavl.svg)](https://readme.cash/c/xx0nyicavl)

# GoogleMapsApi

A friendly, strongly-typed .NET wrapper for the Google Maps Web Services APIs — Geocoding, Routes, Directions, Distance Matrix, Elevation, Time Zone, Places, Address Validation, Solar, Aerial View, Air Quality, Pollen, and Static Maps. Multi-framework (net10.0, net8.0, netstandard2.0 — the latter still covers .NET Framework 4.6.1+), async-first, and battle-tested with **2M+ downloads** on NuGet.

## Supported APIs

| API | Description |
| --- | --- |
| [Geocoding](https://developers.google.com/maps/documentation/geocoding) | Convert between addresses and geographic coordinates |
| [Routes](https://developers.google.com/maps/documentation/routes) | Modern route planning — real-time traffic, eco-routing, toll calc, two-wheeled vehicles (replaces Directions) |
| [Directions](https://developers.google.com/maps/documentation/directions) | Legacy route planning between two points with multiple travel modes |
| [Distance Matrix](https://developers.google.com/maps/documentation/distance-matrix) | Travel time and distance between multiple origins/destinations |
| [Elevation](https://developers.google.com/maps/documentation/elevation) | Elevation data for individual locations or paths |
| [Time Zone](https://developers.google.com/maps/documentation/timezone) | Time zone information for any coordinate |
| [Places (New)](https://developers.google.com/maps/documentation/places/web-service/op-overview) | Modern Places API — Text Search, Nearby Search, Place Details, Autocomplete, Place Photos |
| [Address Validation](https://developers.google.com/maps/documentation/address-validation) | Validate a postal address with component-level confirmation; USPS CASS for US/PR |
| [Solar](https://developers.google.com/maps/documentation/solar) | Building solar potential, roof geometry, panel layouts, financial analyses, and raster data layers (billable) |
| [Aerial View](https://developers.google.com/maps/documentation/aerial-view) | Render and look up cinematic flyover videos for US addresses |
| [Air Quality](https://developers.google.com/maps/documentation/air-quality) | Current conditions, hourly forecast and history, plus heatmap tiles, for a coordinate (billable) |
| [Pollen](https://developers.google.com/maps/documentation/pollen) | Up to 5 days of daily pollen forecast (types and plants) plus heatmap tiles, for a coordinate (billable) |
| [Static Maps](https://developers.google.com/maps/documentation/maps-static) | Generate URLs for static map images with markers, paths, and styles |

## Why this vs Google's official packages

Google ships official .NET packages primarily for its *newer* gRPC APIs — `Google.Maps.Routing.V2`, `Google.Maps.Places.V1`, `Google.Maps.AddressValidation.V1`, `Google.Maps.Geocode.V4`, and friends. For several classic REST web-service APIs (Distance Matrix, Elevation, Time Zone, Directions, Static Maps) **there is no official .NET client at all** — Google's maintained web-service client libraries cover only Java, Python, Go, and Node.js. Where both options exist, here's the honest trade-off:

| Dimension | GoogleMapsApi | Google's official `.V*` packages |
| --- | --- | --- |
| Classic REST web APIs (Distance Matrix, Elevation, Time Zone, Directions, Static Maps) | Typed support | **No official .NET client exists** |
| Packaging | One package (+ an optional DI package) | One NuGet **per API** |
| API surface | Hand-written, idiomatic C# request/response types | gRPC/protobuf-generated message types |
| Runtime dependencies | Lightweight: `System.Text.Json` on modern .NET; small compatibility helpers on `netstandard2.0` | gRPC stack: `Google.Api.Gax.Grpc`, `Google.Geo.Type`, Protobuf/gRPC dependencies; `Grpc.Core` on .NET Framework |
| Maturity | Stable 2.x, 2M+ downloads | Several Maps packages still in beta (`1.0.0-betaNN`) |
| DI / `IHttpClientFactory` | [`AddGoogleMaps(...)`](#instance-based-client-ihttpclientfactory-friendly) extension | `ClientBuilder` pattern; no `IHttpClientFactory` story |
| Observability | [OpenTelemetry](#observability-opentelemetry-tracing) span per call (API key redacted) | None built-in |

**Prefer Google's official packages when** you need gRPC transport or streaming, deep integration with other Google Cloud client libraries, or Google's own support — and you only consume one of the gRPC-backed APIs. Otherwise, a single idiomatic package that also covers the web-service APIs is usually the friendlier choice.

# Installation

Install via NuGet Package Manager:
```
Install-Package GoogleMapsApi
```

Or via .NET CLI:
```
dotnet add package GoogleMapsApi
```

Looking for runnable examples? See [`samples/`](samples/) — console, ASP.NET Core minimal API, and Blazor Server — or the [interactive notebooks](samples/notebooks/) (one live, runnable `.dib` per API surface).

## Scaffold a project in 10 seconds

Spin up a working ASP.NET Core Web API (with `/geocode` and `/directions` endpoints) using the `dotnet new` template:

```
dotnet new install GoogleMapsApi.Templates
dotnet new googlemaps-webapi -o MyMapsApi --apikey YOUR_API_KEY
cd MyMapsApi && dotnet run
```

The key is written to `appsettings.Development.json` (gitignored), and the generated project references the matching `GoogleMapsApi` version. Pass `-f net8.0` to target .NET 8.

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

> [!IMPORTANT]
> The static `GoogleMaps` facade was **removed in 2.0.0**, along with the legacy Places API (use Places (New)). Use the instance-based `IGoogleMapsClient` / `GoogleMapsClient` instead — construct it with an `HttpClient` (directly or via `IHttpClientFactory` / dependency injection). See [Instance-based client](#instance-based-client-ihttpclientfactory-friendly) below. Upgrading from 1.x? See the **[2.0 migration guide](MIGRATION.md)**.

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

// Create a client backed by an HttpClient (reuse a single instance; IHttpClientFactory friendly)
using var http = new HttpClient();
var maps = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "your-google-maps-api-key" });

// Directions
DirectionsRequest directionsRequest = new DirectionsRequest()
{
    Origin = "NYC, 5th and 39",
    Destination = "Philadelphia, Chestnut and Walnut",
};

// Async call (recommended)
DirectionsResponse directions = await maps.Directions.QueryAsync(directionsRequest);
Console.WriteLine(directions);

// Geocode
GeocodingRequest geocodeRequest = new GeocodingRequest()
{
    Address = "new york city",
};
GeocodingResponse geocode = await maps.Geocode.QueryAsync(geocodeRequest);
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

using var http = new HttpClient();
var maps = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "your-google-maps-api-key" });

var request = new RoutesRequest
{
    Origin = Waypoint.FromAddress("San Francisco, CA"),
    Destination = Waypoint.FromAddress("Mountain View, CA"),
    TravelMode = RoutesTravelMode.Drive,
    RoutingPreference = RoutingPreference.TrafficAware,
    // FieldMask defaults to a Directions-equivalent shape; override to slim the response.
};

var response = await maps.Routes.QueryAsync(request);
var route = response.Routes![0];
Console.WriteLine($"{route.DistanceMeters} m, {route.DurationSeconds} s");
```

### Solar API (building solar potential)

The Solar API returns a building's solar potential — roof geometry, panel layouts, expected energy
production, and financial analyses — plus downloadable raster data layers (DSM, flux, shade). It is
a **billable** API, so calls beyond the free tier incur charges.

``` C#
using GoogleMapsApi;
using GoogleMapsApi.Entities.Solar.Request;

using var http = new HttpClient();
var maps = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "your-google-maps-api-key" });

// Solar potential for the building closest to a coordinate.
var insights = await maps.SolarBuildingInsights.QueryAsync(new BuildingInsightsRequest
{
    Latitude = 37.4450,
    Longitude = -122.1390,
});
Console.WriteLine($"Up to {insights.SolarPotential!.MaxArrayPanelsCount} panels");

// Discover raster data layers, then download one as raw GeoTIFF bytes.
var layers = await maps.SolarDataLayers.QueryAsync(new DataLayersRequest
{
    Latitude = 37.4450,
    Longitude = -122.1390,
    RadiusMeters = 50,
});
var dsm = await maps.SolarGeoTiff.QueryAsync(new GeoTiffRequest { Url = layers.DsmUrl! });
await File.WriteAllBytesAsync("dsm.tiff", dsm.Content);
```

### Aerial View API (cinematic flyover videos)

The [Aerial View API](https://developers.google.com/maps/documentation/aerial-view) renders cinematic 3D flyover videos of US addresses. It has two operations, grouped under `maps.AerialView`: `RenderVideo` enqueues rendering (free), and `LookupVideo` fetches a video's state and signed media URIs (billable). Rendering is asynchronous and can take up to a few hours, so the typical flow is *render once, then poll lookup by `videoId` with exponential backoff until the state is `Active`*.

``` C#
using GoogleMapsApi;
using GoogleMapsApi.Entities.AerialView.Request;
using GoogleMapsApi.Entities.AerialView.Response;

using var http = new HttpClient();
var maps = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "your-google-maps-api-key" });

// 1. Request a render (returns immediately; usually Processing on first call).
var render = await maps.AerialView.RenderVideo.QueryAsync(
    new RenderVideoRequest { Address = "500 W 2nd St, Austin, TX 78701" });
var videoId = render.Metadata!.VideoId!;

// 2. Poll until the video is ready (use a real backoff; rendering can take hours).
var video = await maps.AerialView.LookupVideo.QueryAsync(new LookupVideoRequest { VideoId = videoId });

if (video.State == VideoState.Active && video.TryGetUris(MediaFormat.Mp4High, out var uris))
    Console.WriteLine(uris!.LandscapeUri);
```

> A looked-up video that does not exist (or has no 3D imagery available) returns HTTP 404, surfaced as an `HttpRequestException`. A still-rendering video is **not** an error — it returns `State == VideoState.Processing`.

### Air Quality API (current conditions, forecast, history, heatmap tiles)

The [Air Quality API](https://developers.google.com/maps/documentation/air-quality) reports air-quality indexes, pollutant concentrations and health recommendations for a coordinate — as current conditions, an hourly forecast, or hourly history — plus PNG heatmap tiles. Opt into the richer fields with `ExtraComputations`. It is a **billable** API, so calls beyond the free tier incur charges.

``` C#
using GoogleMapsApi;
using GoogleMapsApi.Entities.AirQuality.Request;

using var http = new HttpClient();
var maps = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "your-google-maps-api-key" });

// Current conditions, with health advice and pollutant concentrations.
var current = await maps.AirQualityCurrentConditions.QueryAsync(new CurrentConditionsRequest
{
    Latitude = 37.4220,
    Longitude = -122.0841,
    ExtraComputations = new() { ExtraComputation.HealthRecommendations, ExtraComputation.PollutantConcentration },
});
Console.WriteLine($"{current.Indexes![0].DisplayName}: {current.Indexes[0].Aqi} ({current.Indexes[0].Category})");

// Hourly forecast (paged), and a heatmap tile as raw PNG bytes.
var forecast = await maps.AirQualityForecast.QueryAsync(new ForecastRequest
{
    Latitude = 37.4220,
    Longitude = -122.0841,
    PageSize = 6,
});
var tile = await maps.AirQualityHeatmapTile.QueryAsync(new HeatmapTileRequest
{
    MapType = AirQualityMapType.UsAqi,
    Zoom = 4,
    X = 4,
    Y = 6,
});
await File.WriteAllBytesAsync("aqi-tile.png", tile.Content);
```

### Pollen API (daily forecast + heatmap tiles)

The [Pollen API](https://developers.google.com/maps/documentation/pollen) returns up to five days of daily pollen information — index values, in-season flags and descriptions for pollen types (grass, tree, weed) and individual plants — plus PNG heatmap tiles. It is a **billable** API, so calls beyond the free tier incur charges.

``` C#
using GoogleMapsApi;
using GoogleMapsApi.Entities.Pollen.Request;

using var http = new HttpClient();
var maps = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "your-google-maps-api-key" });

// Five-day pollen forecast.
var forecast = await maps.PollenForecast.QueryAsync(new PollenForecastRequest
{
    Latitude = 40.4168,
    Longitude = -3.7038,
    Days = 5,
});
foreach (var type in forecast.DailyInfo![0].PollenTypeInfo!)
    Console.WriteLine($"{type.DisplayName}: {type.IndexInfo?.Category}");

// A pollen heatmap tile as raw PNG bytes.
var tile = await maps.PollenHeatmapTile.QueryAsync(new PollenHeatmapTileRequest
{
    MapType = PollenMapType.TreeUpi,
    Zoom = 4,
    X = 8,
    Y = 6,
});
await File.WriteAllBytesAsync("pollen-tile.png", tile.Content);
```

### Instance-based client (`IHttpClientFactory`-friendly)

`GoogleMapsClient` is the instance-based entry point that accepts an injected `HttpClient`. This is the standard pattern for ASP.NET Core, minimal APIs, and worker services — it plays nicely with `IHttpClientFactory`, per-instance event handlers, and an ambient API key that is auto-filled into requests when not set explicitly.

The companion package `GoogleMapsApi.Extensions.DependencyInjection` provides an `AddGoogleMaps`
extension that registers the client through `IHttpClientFactory` and binds options in one call:

``` bash
dotnet add package GoogleMapsApi.Extensions.DependencyInjection
```

``` C#
// Register once at startup
services.AddGoogleMaps(options => options.ApiKey = "your-google-maps-api-key");
// …or bind from configuration (e.g. a "GoogleMaps" section in appsettings.json):
services.AddGoogleMaps(builder.Configuration.GetSection("GoogleMaps"));

// Inject and use
public class GeocodingService(IGoogleMapsClient maps)
{
    public Task<GeocodingResponse> LookupAsync(string address)
        => maps.Geocode.QueryAsync(new GeocodingRequest { Address = address });
}
```

`AddGoogleMaps` returns an `IHttpClientBuilder`, so you can chain resilience and other
`HttpClient` configuration. Google's APIs throttle with HTTP 429; rather than hand-rolling retries,
add the standard Polly-backed resilience handler from `Microsoft.Extensions.Http.Resilience`:

``` bash
dotnet add package Microsoft.Extensions.Http.Resilience
```

``` C#
services.AddGoogleMaps(options => options.ApiKey = "your-google-maps-api-key")
        .AddStandardResilienceHandler();
```

The standard handler retries transient failures — including **HTTP 429 (throttling)**, 408, and
5xx — with exponential backoff and a circuit breaker, so callers no longer need to wrap calls in
retry logic.

Prefer not to take the extra package? The core library still works with hand-wired DI:

``` C#
services.AddHttpClient<IGoogleMapsClient, GoogleMapsClient>();
services.AddSingleton(new GoogleMapsClientOptions { ApiKey = "your-google-maps-api-key" });
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

The API is async-first. When you must call from a synchronous context, block on the task (prefer `QueryAsync` whenever possible):

``` C#
DirectionsResponse directions = maps.Directions.QueryAsync(directionsRequest).GetAwaiter().GetResult();
Console.WriteLine(directions);
```

## Observability (OpenTelemetry tracing)

Every API call emits a [distributed-tracing](https://opentelemetry.io/docs/concepts/signals/traces/) span from an
[`ActivitySource`](https://learn.microsoft.com/dotnet/core/diagnostics/distributed-tracing) named **`GoogleMapsApi`**.
There is nothing to enable on the library side — the instrumentation is inert (zero allocations) until a listener is
registered, and lights up automatically once your tracing pipeline subscribes to the source.

With OpenTelemetry, add the source by name (the constant `GoogleMapsApi.Diagnostics.GoogleMapsActivity.SourceName`):

``` C#
using OpenTelemetry.Trace;
using GoogleMapsApi.Diagnostics;

builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing
    .AddSource(GoogleMapsActivity.SourceName) // "GoogleMapsApi"
    .AddOtlpExporter());
```

Each span is `Client`-kind, named `GoogleMapsApi <Api>` (e.g. `GoogleMapsApi Geocoding`), with its **duration**
representing call latency. Tags follow OpenTelemetry HTTP semantic conventions plus a small `gmaps.*` set:

| Tag | Example | Notes |
| --- | --- | --- |
| `gmaps.api` | `Geocoding` | The Maps API invoked |
| `http.request.method` | `GET` / `POST` | |
| `server.address` | `maps.googleapis.com` | |
| `url.full` | `…/geocode/json?...&key=REDACTED` | **API key and signature are always redacted** |
| `http.response.status_code` | `200` | |
| `gmaps.response_status` | `OK` / `ZERO_RESULTS` | Google body status, where the API exposes one |

Failures set the span status to `Error` and add an `error.type` tag.

---

*If this library saved you time, please ⭐ the repo — it helps others find it.*
