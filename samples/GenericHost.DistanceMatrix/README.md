# GenericHost.DistanceMatrix

Console app built on the .NET **Generic Host** that resolves an application service from the
container and uses it to compute travel distance and duration between two places via the
Distance Matrix API.

This is the dedicated showcase for the
[`GoogleMapsApi.Extensions.DependencyInjection`](../../GoogleMapsApi.Extensions.DependencyInjection/)
package: `IGoogleMapsClient` is registered once with `AddGoogleMaps(...)` and the ambient API key
is bound from **configuration**, so `TravelTimeService` depends only on `IGoogleMapsClient` and
never sees the key or an `HttpClient`.

## Run

```bash
GOOGLE_API_KEY=your_api_key dotnet run --project samples/GenericHost.DistanceMatrix -- "New York, NY" "Boston, MA"
```

Omit the origin/destination args and it defaults to New York → Boston.

The API key resolves from (in order):

1. The `GOOGLE_API_KEY` environment variable (mapped into `GoogleMaps:ApiKey` in `Program.cs`).
2. The `GoogleMaps:ApiKey` configuration value — `appsettings.json`, user secrets, or a
   `GoogleMaps__ApiKey` environment variable.

## What this demonstrates

- [`AddGoogleMaps(IConfiguration)`](../../GoogleMapsApi.Extensions.DependencyInjection/GoogleMapsServiceCollectionExtensions.cs) —
  binds [`GoogleMapsClientOptions`](../../GoogleMapsApi/GoogleMapsClientOptions.cs) (the ambient API key) from a config section
- Constructor injection of [`IGoogleMapsClient`](../../GoogleMapsApi/IGoogleMapsClient.cs) into a regular application service
- `Host.CreateApplicationBuilder` generic-host wiring with `IServiceCollection` and `ILogger<T>`
- [`DistanceMatrixRequest`](../../GoogleMapsApi/Entities/DistanceMatrix/Request/DistanceMatrixRequest.cs) /
  [`DistanceMatrixResponse`](../../GoogleMapsApi/Entities/DistanceMatrix/Response/DistanceMatrixResponse.cs) with requests that carry **no** API key — it comes from DI

> The Minimal API sample uses the `AddGoogleMaps(Action<GoogleMapsClientOptions>)` overload instead; this sample uses the configuration-binding overload.
