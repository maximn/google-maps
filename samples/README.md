# Samples

Minimal, copy-pasteable demos of [GoogleMapsApi](../GoogleMapsApi/GoogleMapsApi.csproj) across common .NET project types. Each sample references the library by `<ProjectReference>` so you can see them working against tip-of-master.

All samples read the Google Maps API key from the `GOOGLE_API_KEY` environment variable. The Minimal API and Blazor samples also accept `GoogleApiKey` from configuration (e.g. `appsettings.json` or user secrets) as a fallback. Samples target `net10.0` with a `net8.0` fallback for older SDKs.

| Sample | What it demonstrates | Package | Run |
| --- | --- | --- | --- |
| [Console.Geocoding](Console.Geocoding/) | Geocoding an address from a console app | Core `IGoogleMapsClient` | `GOOGLE_API_KEY=... dotnet run --project samples/Console.Geocoding -- "1600 Amphitheatre Parkway"` |
| [MinimalApi.Directions](MinimalApi.Directions/) | ASP.NET Core minimal API returning a directions route summary as JSON, wired up with a single `AddGoogleMaps(options => ...)` call and an ambient API key | [`GoogleMapsApi.Extensions.DependencyInjection`](../GoogleMapsApi.Extensions.DependencyInjection/) | `GOOGLE_API_KEY=... dotnet run --project samples/MinimalApi.Directions` |
| [GenericHost.DistanceMatrix](GenericHost.DistanceMatrix/) | Generic-host console that injects `IGoogleMapsClient` into an application service, with the ambient API key bound from configuration via `AddGoogleMaps(IConfiguration)` | [`GoogleMapsApi.Extensions.DependencyInjection`](../GoogleMapsApi.Extensions.DependencyInjection/) | `GOOGLE_API_KEY=... dotnet run --project samples/GenericHost.DistanceMatrix -- "New York, NY" "Boston, MA"` |
| [Blazor.Geocoding](Blazor.Geocoding/) | Blazor Server page that geocodes an address typed in the browser | Core `IGoogleMapsClient` (manual `AddHttpClient` registration) | `GOOGLE_API_KEY=... dotnet run --project samples/Blazor.Geocoding` |

The **Minimal API** and **Generic Host** samples are the showcases for the [`GoogleMapsApi.Extensions.DependencyInjection`](../GoogleMapsApi.Extensions.DependencyInjection/) package — both register `IGoogleMapsClient` (backed by `IHttpClientFactory`) plus an ambient API key in one `AddGoogleMaps(...)` call, so individual requests don't repeat the key. They demonstrate the two ways to supply the key: Minimal API uses the `Action<GoogleMapsClientOptions>` overload, Generic Host binds it from configuration with the `IConfiguration` overload. The Console and Blazor samples use the core library directly — Blazor shows the manual `AddHttpClient<IGoogleMapsClient, GoogleMapsClient>()` registration when you'd rather not take the extra package.

> Get an API key in the [Google Cloud Console](https://console.cloud.google.com/) and enable the Geocoding and/or Directions API for your project.
