# Serialization

The library uses **`System.Text.Json`** (not Newtonsoft). Two distinct paths:

- **Response deserialization** (all APIs) — driven by one shared options factory.
- **Request body serialization** (POST APIs only) — currently each request builds its own options.

## Shared response options

`GoogleMapsApi/Engine/JsonSerializerConfiguration.cs` → `CreateOptions()` is the single source of truth
the engine uses to deserialize every response. It sets:

- `PropertyNameCaseInsensitive = true`
- `EnumMemberJsonConverterFactory` (enum handling, below)
- `OverviewPolylineJsonConverter` (lazy-decoded polylines)
- `DurationJsonConverter<DistanceMatrix.Response.Duration>` and `…<Directions.Response.Duration>`

`MapsAPIGenericEngine` caches one `JsonSerializerOptions` instance from this and reuses it. Test code
should reuse `JsonSerializerConfiguration.CreateOptions()` too, so prod and tests serialize identically.

## Custom converters (`GoogleMapsApi/Engine/JsonConverters/`)

| Converter | Purpose | Notes |
| --- | --- | --- |
| `EnumMemberJsonConverterFactory` / `EnumMemberJsonConverter<TEnum>` | Map enums to/from their `[EnumMember(Value=…)]` wire strings (or the enum name if none). Also accepts numeric tokens. | Read uses an **`OrdinalIgnoreCase`** string→enum map. Mappings are cached per type in a `ConcurrentDictionary`. |
| `OverviewPolylineJsonConverter` | Decodes Google's encoded polyline into points lazily | Uses reflection on a private member + an `OnDeserialized` callback — fragile to renames (B3). |
| `DurationJsonConverter<T>` | `{ value: seconds, text: string }` ↔ a `Duration` with a `TimeSpan` | Reflects over the `Value`/`Text` properties by name (rename-fragile); a missing token silently leaves that field unset (B3). |

`GoogleMapsApi/Engine/UnixTimeConverter.cs` converts `DateTime` → Unix seconds (one-way) for **legacy
GET query params** (e.g. Directions/Distance Matrix departure time). New POST APIs instead use ISO-8601 strings
(`DateTimeOffset`) in the JSON body.

## Enum handling rules

- Add `[EnumMember(Value = "wire_value")]` to an enum member when Google's string differs from the C#
  name (e.g. `street_address`). If the names already match, no attribute is needed — the converter
  falls back to the member name.
- The factory's `CanConvert` returns true for **every** enum, so any enum on a response is handled
  consistently. Prefer this over per-property `[JsonConverter]` attributes.
- Be aware: case-insensitive matching means `"OK"` and `"ok"` both parse — it won't catch a typo'd
  Google value.

## Request-body serialization (POST APIs)

Routes, Address Validation, and the Places (New) requests each construct their **own**
`new JsonSerializerOptions { DefaultIgnoreCondition = WhenWritingNull, … }` inside their
`GetRequestBody()` and serialize an `internal sealed Payload` shaped with `[JsonPropertyName]`.

> **Known inconsistency (B2):** this duplicates converter/option setup across five request types and
> can drift from the shared response options. When adding a POST API, prefer factoring the body options
> into a shared helper (mirroring `JsonSerializerConfiguration`). Tracked in
> [`known-issues.md`](known-issues.md).

## Query-string building (GET APIs)

`GoogleMapsApi/QueryStringParametersList.cs` collects `key=value` pairs, **silently skips null
values**, and URL-escapes both sides via `Uri.EscapeDataString`. `MapsBaseRequest.GetUri()` joins them
after `json?`.
