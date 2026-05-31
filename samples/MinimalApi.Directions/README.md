# MinimalApi.Directions

ASP.NET Core minimal API that exposes `GET /directions?origin=X&destination=Y` and returns a route summary as JSON.

## Run

```bash
GOOGLE_API_KEY=your_api_key dotnet run --project samples/MinimalApi.Directions
```

Then in another terminal:

```bash
curl 'http://localhost:5000/directions?origin=New+York,NY&destination=Boston,MA'
```

The API key is sourced from the `GOOGLE_API_KEY` env var, falling back to the `GoogleApiKey` config value (e.g. via `appsettings.json` or user secrets).

## What this demonstrates

- [`IGoogleMapsClient`](../../GoogleMapsApi/IGoogleMapsClient.cs) instance client (`.Directions`) registered via `AddHttpClient<IGoogleMapsClient, GoogleMapsClient>()` and injected into the endpoint
- [`DirectionsRequest`](../../GoogleMapsApi/Entities/Directions/Request/DirectionsRequest.cs)
- [`DirectionsResponse`](../../GoogleMapsApi/Entities/Directions/Response/DirectionsResponse.cs) (route summary, legs, distance, duration)
- Async-first usage inside an ASP.NET Core minimal API handler
