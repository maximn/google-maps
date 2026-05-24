# Console.Geocoding

Console app that takes an address (from command-line args or stdin) and prints the geocoded latitude/longitude plus Google's formatted address.

## Run

```bash
GOOGLE_API_KEY=your_api_key dotnet run --project samples/Console.Geocoding -- "1600 Amphitheatre Parkway, Mountain View, CA"
```

Or omit the address and you'll be prompted:

```bash
GOOGLE_API_KEY=your_api_key dotnet run --project samples/Console.Geocoding
```

## What this demonstrates

- [`GoogleMaps.Geocode`](../../GoogleMapsApi/GoogleMaps.cs) facade entry point
- [`GeocodingRequest`](../../GoogleMapsApi/Entities/Geocoding/Request/GeocodingRequest.cs)
- [`GeocodingResponse`](../../GoogleMapsApi/Entities/Geocoding/Response/GeocodingResponse.cs)
- Async-first usage via `QueryAsync`
