# Migrating to GoogleMapsApi 2.0

Version 2.0 is a **breaking** release. It removes the long-deprecated static `GoogleMaps`
facade, removes the legacy (frozen) Places API in favour of **Places API (New)**, trims the
target-framework matrix, and drops a handful of obsolete members.

This guide walks through every breaking change and shows the mechanical fix for each.

- [1. Target frameworks](#1-target-frameworks)
- [2. Static `GoogleMaps` facade removed](#2-static-googlemaps-facade-removed)
- [3. Legacy Places API removed](#3-legacy-places-api-removed)
- [4. Other removed members](#4-other-removed-members)
- [Quick reference](#quick-reference)

---

## 1. Target frameworks

The explicit `net6.0`, `net481`, and `net462` targets were dropped. The library now ships:

| | Before (1.x) | After (2.0) |
| --- | --- | --- |
| Main library | `netstandard2.0;net6.0;net8.0;net10.0;net481;net462` | `netstandard2.0;net8.0;net10.0` |

**.NET Framework 4.6.1+ and .NET 6 consumers are still supported** — they now resolve the
`netstandard2.0` build instead of a framework-specific one. No code change is required; if you
pinned a specific TFM build in tooling, point it at `netstandard2.0`.

---

## 2. Static `GoogleMaps` facade removed

The static `GoogleMaps.*` entry point (deprecated in 1.8.0) is gone. Use the instance-based
`GoogleMapsClient` / `IGoogleMapsClient` instead. It is `IHttpClientFactory`-friendly, supports
per-instance event handlers, and takes an ambient API key via `GoogleMapsClientOptions`.

The migration is mechanical: replace `GoogleMaps.Xxx.QueryAsync(...)` with
`client.Xxx.QueryAsync(...)` on a `GoogleMapsClient` instance.

```csharp
// Before (1.x — removed in 2.0)
var result = await GoogleMaps.Geocode.QueryAsync(request);

// After (2.0)
using var http = new HttpClient();
var maps = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "your-api-key" });

var result = await maps.Geocode.QueryAsync(request);
```

### With dependency injection (recommended)

```csharp
// Registration — IHttpClientFactory creates/manages the HttpClient; we pass the ambient API key.
services.AddHttpClient(nameof(GoogleMapsClient));
services.AddSingleton<IGoogleMapsClient>(sp =>
{
    var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(GoogleMapsClient));
    return new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "your-api-key" });
});

// Consumption
public sealed class TripService(IGoogleMapsClient maps)
{
    public Task<GeocodingResponse> LocateAsync(GeocodingRequest request)
        => maps.Geocode.QueryAsync(request);
}
```

> **API key:** the ambient key on `GoogleMapsClientOptions` is applied to every request that
> doesn't set its own `ApiKey`. A key set explicitly on a request is preserved.

---

## 3. Legacy Places API removed

Google froze the legacy Places web service; 2.0 removes the library's wrappers for it entirely.
All Places functionality now goes through **Places API (New)**, which was added in 1.6.0 and is the
only Places surface in 2.0.

> **Heads-up — `FieldMask`:** Places (New) bills by the fields you request and **requires** a field
> mask. Each new request type exposes a `FieldMask` property with a sensible default; override it to
> request more (or fewer) fields. This has no equivalent in the legacy API.

### Accessor & type mapping

| Removed (legacy) | Replacement (Places New) | Client accessor |
| --- | --- | --- |
| `Places` / `PlacesRequest` → `PlacesResponse` | `SearchNearbyRequest` → `SearchNearbyResponse` | `client.PlacesSearchNearby` |
| `PlacesNearBy` / `PlacesNearByRequest` → `PlacesNearByResponse` | `SearchNearbyRequest` → `SearchNearbyResponse` | `client.PlacesSearchNearby` |
| `PlacesText` / `PlacesTextRequest` → `PlacesTextResponse` | `SearchTextRequest` → `SearchTextResponse` | `client.PlacesSearchText` |
| `PlacesFind` / `PlacesFindRequest` → `PlacesFindResponse` | `SearchTextRequest` → `SearchTextResponse` | `client.PlacesSearchText` |
| `PlacesDetails` / `PlacesDetailsRequest` → `PlacesDetailsResponse` | `PlaceDetailsRequest` → `Place` | `client.PlaceDetailsNew` |
| `PlaceAutocomplete` / `PlaceAutocompleteRequest` → `PlaceAutocompleteResponse` | `AutocompleteRequest` → `AutocompleteResponse` | `client.PlacesAutocompleteNew` |
| _(photo reference on a legacy details result)_ | `PlacePhotoRequest` → `PlacePhotoResponse` | `client.PlacePhoto` |
| `PlacesRadar` / `PlacesRadarRequest` | **None** — Radar Search was discontinued by Google in 2018 | — |

The `GoogleMapsApi.Entities.Places*` namespaces (`Places`, `PlacesText`, `PlacesDetails`,
`PlaceAutocomplete`, `PlacesNearBy`, `PlacesFind`, `PlacesRadar`) and the legacy
`PriceLevelJsonConverter` no longer exist. Replace `using GoogleMapsApi.Entities.PlacesText.*`
(etc.) with `using GoogleMapsApi.Entities.PlacesNew.Request;` /
`using GoogleMapsApi.Entities.PlacesNew.Response;`.

### Text search

```csharp
// Before (1.x — removed)
var request = new PlacesTextRequest
{
    Query = "pizza in New York",
    Language = "en",
    ApiKey = "your-api-key",
};
PlacesTextResponse response = await GoogleMaps.PlacesText.QueryAsync(request);

// After (2.0)
var request = new SearchTextRequest
{
    TextQuery = "pizza in New York",
    LanguageCode = "en",
    // FieldMask defaults to a useful set; override to request more fields.
};
SearchTextResponse response = await maps.PlacesSearchText.QueryAsync(request);
```

Property renames worth noting: `Query` → `TextQuery`, `Language` → `LanguageCode`, and the
legacy `Location` + `Radius` pair is expressed as a `LocationBias` (circle) or
`LocationRestriction`.

### Place details

```csharp
// Before (1.x — removed)
var request = new PlacesDetailsRequest
{
    PlaceId = "ChIJN1t_tDeuEmsRUsoyG83frY4",
    Language = "en",
    ApiKey = "your-api-key",
};
PlacesDetailsResponse response = await GoogleMaps.PlacesDetails.QueryAsync(request);
var name = response.Result.Name;

// After (2.0) — note the response IS the Place (no wrapper / Status / Result)
var request = new PlaceDetailsRequest
{
    PlaceId = "ChIJN1t_tDeuEmsRUsoyG83frY4",
    LanguageCode = "en",
};
Place place = await maps.PlaceDetailsNew.QueryAsync(request);
var name = place.DisplayName?.Text;   // DisplayName is a LocalizedText
```

### Autocomplete

```csharp
// Before (1.x — removed)
var request = new PlaceAutocompleteRequest { Input = "1600 Amphi", ApiKey = "your-api-key" };
PlaceAutocompleteResponse response = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

// After (2.0)
var request = new AutocompleteRequest { Input = "1600 Amphi" };
AutocompleteResponse response = await maps.PlacesAutocompleteNew.QueryAsync(request);
```

---

## 4. Other removed members

These were obsolete and are deleted in 2.0:

| Removed | Replacement |
| --- | --- |
| `MapsBaseRequest.Sensor` | None — Google dropped the `sensor` parameter years ago; it was never sent. Delete the assignment. |
| `StaticMapRequest.Sensor` | None — same as above. |
| `TimeZoneResponse.OffSet` | Use `TimeZoneResponse.DstOffSet` (same value; `OffSet` was an alias). |

---

## Quick reference

```diff
- var r = await GoogleMaps.Geocode.QueryAsync(req);
+ using var http = new HttpClient();
+ var maps = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "..." });
+ var r = await maps.Geocode.QueryAsync(req);

- await GoogleMaps.PlacesText.QueryAsync(new PlacesTextRequest    { Query = q })
+ await maps.PlacesSearchText.QueryAsync(new SearchTextRequest    { TextQuery = q })

- await GoogleMaps.PlacesDetails.QueryAsync(new PlacesDetailsRequest { PlaceId = id })
+ await maps.PlaceDetailsNew.QueryAsync(new PlaceDetailsRequest     { PlaceId = id })

- await GoogleMaps.PlaceAutocomplete.QueryAsync(new PlaceAutocompleteRequest { Input = i })
+ await maps.PlacesAutocompleteNew.QueryAsync(new AutocompleteRequest         { Input = i })

- response.DstOffSet  // via TimeZoneResponse.OffSet (removed)
+ response.DstOffSet  // unchanged property, use directly
```

Still stuck? Open an issue at <https://github.com/maximn/google-maps/issues>.
