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

## API key configuration

You can configure your Google Maps API key in several ways. The simplest is to set it directly on each request:

```csharp
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

GeocodingRequest geocodeRequest = new GeocodingRequest()
{
    Address = "new york city",
    ApiKey = "your-google-maps-api-key"
};

GeocodingResponse geocode = await GoogleMaps.Geocode.QueryAsync(geocodeRequest);
Console.WriteLine(geocode);
```

For more configuration options (including global configuration via `app.config` / `appsettings.json`), see the [wiki](https://github.com/maximn/google-maps/wiki).

## Synchronous usage

Synchronous calls are also supported via `Query` — prefer `QueryAsync` whenever possible:

```csharp
GeocodingResponse geocode = GoogleMaps.Geocode.Query(geocodeRequest);
Console.WriteLine(geocode);
```

## Next steps

- Browse the [API Reference](../api/GoogleMapsApi.yml) for the full surface area of every supported service.
- See the project [README](https://github.com/maximn/google-maps#readme) for a broader code tour, including Directions and Static Maps examples.
