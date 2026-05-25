# Getting Started

This guide walks you through installing **GoogleMapsApi** and making your first geocoding request.

## Installation

Install via NuGet Package Manager:

```
Install-Package GoogleMapsApi
```

Or via the .NET CLI:

```
dotnet add package GoogleMapsApi
```

## Your first request

Construct a single `GoogleMapsClient` per process (typically via `IHttpClientFactory`) and reuse it across calls:

```csharp
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

using var http = new HttpClient();
IGoogleMapsClient maps = new GoogleMapsClient(http, new GoogleMapsClientOptions
{
    ApiKey = "your-google-maps-api-key",
});

GeocodingResponse response = await maps.Geocode.QueryAsync(new GeocodingRequest
{
    Address = "new york city",
});

Console.WriteLine(response);
```

## API key — two ways to provide it

```csharp
// Option 1: ambient default on the client (auto-filled into every request
// that doesn't set its own ApiKey)
new GoogleMapsClientOptions { ApiKey = "..." };

// Option 2: per-request override (wins over the ambient default)
new GeocodingRequest { Address = "...", ApiKey = "another-key" };
```

## Dependency injection

In ASP.NET Core, minimal APIs, and worker services, register `GoogleMapsClient` once at startup:

```csharp
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IGoogleMapsClient>(sp => new GoogleMapsClient(
    sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(GoogleMapsClient)),
    new GoogleMapsClientOptions { ApiKey = builder.Configuration["GoogleMaps:ApiKey"] }));
```

Then inject `IGoogleMapsClient` wherever you need it.

> Coming in 2.1: `services.AddGoogleMaps(o => o.ApiKey = ...)` collapses this to one call.

## Synchronous usage

Sync overloads exist on `IEngineFacade<>` and block the calling thread via `GetAwaiter().GetResult()`. Prefer `QueryAsync` whenever possible — sync calls can deadlock on single-threaded SynchronizationContexts (classic ASP.NET, WinForms, WPF UI threads).

```csharp
GeocodingResponse response = maps.Geocode.Query(new GeocodingRequest { Address = "..." });
```

## Next steps

- Browse the [API Reference](../api/GoogleMapsApi.yml) for the full surface area of every supported service.
- See the project [README](https://github.com/maximn/google-maps#readme) for Directions, Distance Matrix, Places, and Static Maps examples.
