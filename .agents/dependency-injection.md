# Dependency injection & samples

DI lives in a **separate NuGet package**, `GoogleMapsApi.Extensions.DependencyInjection` — it is **not**
bundled into the core `GoogleMapsApi` package. Consumers opt in, or wire `IHttpClientFactory` manually.

## `AddGoogleMaps` (`GoogleMapsServiceCollectionExtensions.cs`)

Three overloads, all returning `IHttpClientBuilder` (so you can chain resilience handlers, etc.):

```csharp
services.AddGoogleMaps();                                  // no ambient key; set ApiKey per request
services.AddGoogleMaps(o => o.ApiKey = "…");               // ambient key via Action<GoogleMapsClientOptions>
services.AddGoogleMaps(configuration.GetSection("GoogleMaps"));  // bind options from config
```

Under the hood (`AddCore`):

```csharp
services.AddHttpClient<IGoogleMapsClient, GoogleMapsClient>()
        .AddTypedClient<IGoogleMapsClient>((httpClient, sp) =>
            new GoogleMapsClient(httpClient, sp.GetRequiredService<IOptions<GoogleMapsClientOptions>>().Value));
```

So `IGoogleMapsClient` is a **typed client backed by `IHttpClientFactory`**. Inject `IGoogleMapsClient`
into your services and call `maps.Geocode.QueryAsync(...)`, etc.

### Gotchas

- **Config binds from a section, not the root.** The third overload expects `ApiKey` at the section
  root — pass `configuration.GetSection("GoogleMaps")`, where the config has `GoogleMaps:ApiKey`.
- **Last call wins.** Each overload (re)configures `GoogleMapsClientOptions`; calling `AddGoogleMaps`
  twice overwrites, it doesn't accumulate.
- **Don't dispose the resolved client.** `IHttpClientFactory` owns the underlying `HttpClient` lifetime
  and socket pooling; let the container manage it. Never `new HttpClient()` per request.
- **Ambient key is per-request fill, not global state** — see the ambient-key section in
  [`architecture.md`](architecture.md).

## Versioning

Core + DI packages are versioned in **lockstep** from the same `v*` git tag (MinVer), and the DI package
depends on the matching core version. See [`build-release-ci.md`](build-release-ci.md).

## Samples (`samples/`)

A ladder from raw to best-practice — use them as copy-paste starting points:

| Sample | Pattern shown |
| --- | --- |
| `Console.Geocoding` | No DI — `new GoogleMapsClient(new HttpClient())` (simplest; not the production pattern) |
| `Blazor.Geocoding` | Manual `AddHttpClient<IGoogleMapsClient, GoogleMapsClient>()` **without** the DI package |
| `MinimalApi.Directions` | `AddGoogleMaps(o => o.ApiKey = …)` — action-based config |
| `GenericHost.DistanceMatrix` | `AddGoogleMaps(IConfiguration)` — section binding + a service consuming `IGoogleMapsClient` |

(Sample-level nits — capturing the API key in a closure, a singleton holding the typed client — are
noted in [`known-issues.md`](known-issues.md); they're illustrative, not load-bearing.)
