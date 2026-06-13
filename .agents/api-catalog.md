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
| `PlaceDetailsNew` | `PlaceDetailsRequest` / `Place` | POST | `MapsBaseRequest` | Places (New) |
| `PlacesAutocompleteNew` | `AutocompleteRequest` / `AutocompleteResponse` | POST | `MapsBaseRequest` | Places (New) |
| `PlacePhoto` | `PlacePhotoRequest` / `PlacePhotoResponse` | POST | `MapsBaseRequest` | Places (New) |

Entities live under `GoogleMapsApi/Entities/{Service}/{Request|Response|Common}/`.

## Legacy → new map

2.0 migrated to Google's current APIs and **removed the frozen legacy Places API**:

| Old | New in this library |
| --- | --- |
| Directions API | **Routes API** (`Routes`) — real-time traffic, tolls, eco-routing, alternatives |
| Places API (find/nearby/text/details/autocomplete/photo) | **Places (New)** (`Places*`, `PlaceDetailsNew`, `PlacePhoto`) |
| (n/a) | **Address Validation API** (`AddressValidation`) |

`Directions`, `DistanceMatrix`, `Geocoding`, `Elevation`, `TimeZone` remain as the established GET APIs.
See [`MIGRATION.md`](../MIGRATION.md) for the consumer-facing upgrade guide.

## URL signing (legacy GET only)

Legacy requests derive from `SignableRequest` (`Entities/Common/SignableRequest.cs`): set `ClientID`
(must start with `gme-`) + `SigningKey` to sign the URL with HMAC-SHA1 (Google Enterprise). The new
POST APIs derive from `MapsBaseRequest` directly and **do not support signing** — API key only.

## Routes field mask

The Routes API **requires** a response field mask (sent as the `$fields` URL param).
`RoutesRequest.FieldMask` defaults to `RoutesRequest.DefaultFieldMask` (roughly what Directions
returned). Tighten it to cut response size and cost. An empty mask is rejected by Google.

## Gotchas

- **Inconsistent SSL enforcement.** `TimeZoneRequest` overrides `IsSSL` to force HTTPS, while other
  legacy requests let you set `IsSSL = false`. (All requests still reject an API key over plain HTTP.)
  Tracked in [`known-issues.md`](known-issues.md) (B7).
- **`Routes`/`AddressValidation` ignore `Channel`/signing** — they don't extend `SignableRequest`.
- Don't confuse `Directions` (legacy GET) with `Routes` (new POST) — they have different entity trees.
