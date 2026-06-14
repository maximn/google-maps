# API catalog

The services exposed by `IGoogleMapsClient` (`GoogleMapsApi/IGoogleMapsClient.cs`), how they map to
Google's APIs, and the legacy → new migration story. Static Maps is separate (URL builder, not an
HTTP call) under `GoogleMapsApi/StaticMaps/`.

## Services on `IGoogleMapsClient`

| Property | Request / Response | Verb | Base class | Notes |
| --- | --- | --- | --- | --- |
| `Geocode` | `GeocodingRequest` / `GeocodingResponse` | GET | `SignableRequest` | Address ↔ coordinates |
| `Directions` | `DirectionsRequest` / `DirectionsResponse` | GET | `SignableRequest` | Legacy; prefer `Routes` for new work |
| `DistanceMatrix` | `DistanceMatrixRequest` / `DistanceMatrixResponse` | GET | `SignableRequest` | Travel time/distance grid |
| `Elevation` | `ElevationRequest` / `ElevationResponse` | GET | `SignableRequest` | Elevation for points/paths |
| `TimeZone` | `TimeZoneRequest` / `TimeZoneResponse` | GET | `SignableRequest` | Forces SSL (see gotcha) |
| `Routes` | `RoutesRequest` / `RoutesResponse` | POST | `MapsBaseRequest` | **Modern replacement for Directions**; requires a field mask |
| `AddressValidation` | `AddressValidationRequest` / `AddressValidationResponse` | POST | `MapsBaseRequest` | USPS CASS for US/PR |
| `PlacesSearchText` | `SearchTextRequest` / `SearchTextResponse` | POST | `MapsBaseRequest` | Places (New) |
| `PlacesSearchNearby` | `SearchNearbyRequest` / `SearchNearbyResponse` | POST | `MapsBaseRequest` | Places (New) |
| `PlaceDetailsNew` | `PlaceDetailsRequest` / `Place` | GET | `MapsBaseRequest` | Places (New); GET (no request body) |
| `PlacesAutocompleteNew` | `AutocompleteRequest` / `AutocompleteResponse` | POST | `MapsBaseRequest` | Places (New) |
| `PlacePhoto` | `PlacePhotoRequest` / `PlacePhotoResponse` | GET | `MapsBaseRequest` | Places (New); GET (no request body) |
| `SnapToRoads` | `SnapToRoadsRequest` / `SnapToRoadsResponse` | GET | `MapsBaseRequest` | Roads API; snap a GPS trace to road segments |
| `NearestRoads` | `NearestRoadsRequest` / `NearestRoadsResponse` | GET | `MapsBaseRequest` | Roads API; nearest segment for each point |
| `SpeedLimits` | `SpeedLimitsRequest` / `SpeedLimitsResponse` | GET | `MapsBaseRequest` | Roads API; needs Asset Tracking license (else 403), billable per limit |
| `SolarBuildingInsights` | `BuildingInsightsRequest` / `BuildingInsightsResponse` | GET | `MapsBaseRequest` | Solar API (billable); forces SSL |
| `SolarDataLayers` | `DataLayersRequest` / `DataLayersResponse` | GET | `MapsBaseRequest` | Solar API (billable); forces SSL |
| `SolarGeoTiff` | `GeoTiffRequest` / `GeoTiffResponse` | GET | `MapsBaseRequest` | Solar API (billable); raw GeoTIFF bytes, forces SSL |
| `AerialView` | `IAerialViewApi` (sub-API) | — | — | Aerial View API; two facades, see below |

`AerialView` is a sub-interface (`GoogleMapsApi/IAerialViewApi.cs`) rather than a single facade; both
operations share the `AerialViewVideoResponse` type:

| Facade | Request / Response | Verb | Base class | Notes |
| --- | --- | --- | --- | --- |
| `AerialView.RenderVideo` | `RenderVideoRequest` / `AerialViewVideoResponse` | POST | `MapsBaseRequest` | Enqueue a flyover render for a US address (free); poll via `LookupVideo` |
| `AerialView.LookupVideo` | `LookupVideoRequest` / `AerialViewVideoResponse` | GET | `MapsBaseRequest` | Fetch a video by id or address (billable); returns state + signed media URIs |

Entities live under `GoogleMapsApi/Entities/{Service}/{Request|Response|Common}/`.

## Legacy → new map

2.0 migrated to Google's current APIs and **removed the frozen legacy Places API**:

| Old | New in this library |
| --- | --- |
| Directions API | **Routes API** (`Routes`) — real-time traffic, tolls, eco-routing, alternatives |
| Places API (find/nearby/text/details/autocomplete/photo) | **Places (New)** (`Places*`, `PlaceDetailsNew`, `PlacePhoto`) |
| (n/a) | **Address Validation API** (`AddressValidation`) |
| (n/a) | **Roads API** (`SnapToRoads`, `NearestRoads`, `SpeedLimits`) |
| (n/a) | **Solar API** (`SolarBuildingInsights`, `SolarDataLayers`, `SolarGeoTiff`) |
| (n/a) | **Aerial View API** (`AerialView`) |

`Directions`, `DistanceMatrix`, `Geocoding`, `Elevation`, `TimeZone` remain as the established GET APIs.
See [`MIGRATION.md`](../MIGRATION.md) for the consumer-facing upgrade guide.

## URL signing (legacy GET only)

Only the five legacy GET APIs (`Geocode`, `Directions`, `DistanceMatrix`, `Elevation`, `TimeZone`)
derive from `SignableRequest` (`Entities/Common/SignableRequest.cs`): set `ClientID` (must start with
`gme-`) + `SigningKey` to sign the URL with HMAC-SHA1 (Google Enterprise). Every newer API —
**regardless of verb** (POST: Routes, Places search/autocomplete, Address Validation, Aerial View
render; GET: Place Details/Photo, Roads, Solar, Aerial View lookup) — derives from `MapsBaseRequest`
directly and **does not support signing**: API key only. So "GET vs POST" no longer tells you whether a
request is signable — the base class does.

## Routes field mask

The Routes API **requires** a response field mask (sent as the `$fields` URL param).
`RoutesRequest.FieldMask` defaults to `RoutesRequest.DefaultFieldMask` (roughly what Directions
returned). Tighten it to cut response size and cost. An empty mask is rejected by Google.

## Gotchas

- **Inconsistent SSL enforcement.** `TimeZoneRequest` overrides `IsSSL` to force HTTPS, while other
  legacy requests let you set `IsSSL = false`. (All requests still reject an API key over plain HTTP.)
  Tracked in [`known-issues.md`](known-issues.md) (B7).
- **The newer APIs ignore `Channel`/signing** — `Routes`, `AddressValidation`, Places, `Roads*`,
  `Solar*` and `AerialView` don't extend `SignableRequest`.
- **Places (New) is mixed-verb.** `PlacesSearchText`, `PlacesSearchNearby`, and `PlacesAutocompleteNew`
  POST a JSON body, but `PlaceDetailsNew` and `PlacePhoto` are **GET** (they override only `GetUri()`,
  so `GetRequestBody()` returns `null` → the engine issues a GET). Don't assume "Places (New) ⇒ POST".
- **Solar requests force SSL** (`GetUri` throws when `IsSSL = false`), same as `TimeZoneRequest`.
- **`SpeedLimits` requires a Google Asset Tracking license** — other keys get an HTTP 403, and each
  returned limit is separately billable.
- **`AerialView` is a sub-API** (`IAerialViewApi`): `RenderVideo` (POST, free) enqueues a render,
  `LookupVideo` (GET, billable) polls it; both return `AerialViewVideoResponse`.
- Don't confuse `Directions` (legacy GET) with `Routes` (new POST) — they have different entity trees.
