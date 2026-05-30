# Blazor.Geocoding

Blazor Server app with a single page: type an address and see the geocoded coordinates. Server-side rendering keeps the Google API key off the client.

## Run

```bash
GOOGLE_API_KEY=your_api_key dotnet run --project samples/Blazor.Geocoding
```

Open the URL the host prints (typically `https://localhost:5001`).

The API key is sourced from the `GOOGLE_API_KEY` env var, falling back to the `GoogleApiKey` config value (e.g. via `appsettings.json` or user secrets).

## What this demonstrates

- [`IGoogleMapsClient`](../../GoogleMapsApi/IGoogleMapsClient.cs) instance client (`.Geocode`) registered via `AddHttpClient<IGoogleMapsClient, GoogleMapsClient>()` and injected into a service
- [`GeocodingRequest`](../../GoogleMapsApi/Entities/Geocoding/Request/GeocodingRequest.cs)
- [`GeocodingResponse`](../../GoogleMapsApi/Entities/Geocoding/Response/GeocodingResponse.cs)
- Wrapping the library in a DI-injected service consumed by a Blazor Server component
