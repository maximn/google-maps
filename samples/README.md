# Samples

Minimal, copy-pasteable demos of [GoogleMapsApi](../GoogleMapsApi/GoogleMapsApi.csproj) across common .NET project types. Each sample references the library by `<ProjectReference>` so you can see them working against tip-of-master.

All samples read the Google Maps API key from the `GOOGLE_API_KEY` environment variable. The Minimal API and Blazor samples also accept `GoogleApiKey` from configuration (e.g. `appsettings.json` or user secrets) as a fallback. Samples target `net10.0` with a `net8.0` fallback for older SDKs.

| Sample | What it demonstrates | Run |
| --- | --- | --- |
| [Console.Geocoding](Console.Geocoding/) | Geocoding an address from a console app | `GOOGLE_API_KEY=... dotnet run --project samples/Console.Geocoding -- "1600 Amphitheatre Parkway"` |
| [MinimalApi.Directions](MinimalApi.Directions/) | ASP.NET Core minimal API returning a directions route summary as JSON | `GOOGLE_API_KEY=... dotnet run --project samples/MinimalApi.Directions` |
| [Blazor.Geocoding](Blazor.Geocoding/) | Blazor Server page that geocodes an address typed in the browser | `GOOGLE_API_KEY=... dotnet run --project samples/Blazor.Geocoding` |

> Get an API key in the [Google Cloud Console](https://console.cloud.google.com/) and enable the Geocoding and/or Directions API for your project.
