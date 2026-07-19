# Adding or extending an API

The highest-frequency change in this repo. Read [`architecture.md`](architecture.md) first for the
request/response contract. Use an existing pair as a template:
- **GET (legacy-style):** `Entities/Geocoding/Request/GeocodingRequest.cs` + `Entities/Geocoding/Response/GeocodingResponse.cs`
- **POST (new-style):** `Entities/Routes/Request/RoutesRequest.cs` + `Entities/Routes/Response/RoutesResponse.cs`

## Decide: GET or POST?

| | Legacy GET APIs | New POST APIs |
| --- | --- | --- |
| Examples | Geocoding, Directions, Distance Matrix, Elevation, Time Zone | Routes, Address Validation, Places (New) |
| Base class | `SignableRequest` (gets URL-signing + `Channel`) | `MapsBaseRequest` directly (API key only) |
| Override | `GetQueryStringParameters()` | `GetRequestBody()` (return JSON `HttpContent`) |
| Endpoint | adds a path segment + `json?…` | full host + path, often needs a **field mask** |

If Google offers the API as a POST/JSON service (the modern style), follow the POST pattern. Don't add
new surface to the frozen legacy APIs unless Google added a parameter to them.

## Steps

1. **Create the entities folder** `GoogleMapsApi/Entities/{Service}/Request/` and `…/Response/`.
   Group shared types under `…/{Service}/Common/` (see `Entities/PlacesNew/Common/`).

2. **Request class** — `public sealed class {Service}Request : MapsBaseRequest` (or `: SignableRequest`
   for a legacy GET).
   - **GET:** override `BaseUrl` to append the path, and `GetQueryStringParameters()` — start from
     `base.GetQueryStringParameters()` (that's what adds the `key`), then `Add(...)` your params.
     Validate required inputs and `throw new ArgumentException(...)` early (see `GeocodingRequest`).
     Do **not** add an SSL/`IsSSL` check — HTTPS is enforced centrally in `MapsBaseRequest`, and
     `ConsistencyTests` fails if a request redeclares its own SSL knob.
   - **POST:** override `GetRequestBody()` to return an `HttpContent` (JSON). Use an `internal sealed`
     `Payload` class with `[JsonPropertyName]` attributes for the wire shape. If the endpoint needs a
     field mask (Routes does), expose it as a property with a sensible `Default…` constant
     (`RoutesRequest.DefaultFieldMask`) and send it as the `$fields` URL param. See
     [`serialization.md`](serialization.md) for how to build the body options consistently.

3. **Response class** — `public class {Service}Response : IResponseFor<{Service}Request>`. Use
   `[JsonPropertyName]` for camelCase fields. If it has a top-level `Status`, the engine will surface
   it as the `gmaps.response_status` trace tag automatically (reflection over a `Status` property).
   Add `[JsonConverter]`/`[EnumMember]` as needed — see [`serialization.md`](serialization.md).

4. **Expose it on the client** — add the property to **both**:
   - `GoogleMapsApi/IGoogleMapsClient.cs` — `IEngineFacade<{Service}Request,{Service}Response> {Name} { get; }`
   - `GoogleMapsApi/GoogleMapsClient.cs` — initialize it in the constructor:
     `{Name} = new HttpClientEngineFacade<{Service}Request,{Service}Response>(httpClient, options);`

5. **Update the public-API lock** — new public types/members go in
   `GoogleMapsApi/PublicAPI.Unshipped.txt` (sorted, with the `#nullable enable` header) or the build
   fails with `RS0016`. Full workflow in [`public-api.md`](public-api.md).

6. **Tests** — a hermetic unit test (mock the HTTP handler; assert the URL/body it builds and that a
   canned JSON payload deserializes) **and** a live integration test extending `BaseTestIntegration`.
   If the API is billable, tag the fixture `[BillableTest]`. See [`testing.md`](testing.md).

7. **DI** needs no change — `GoogleMapsClient` is registered as a whole, so a new property is available
   automatically through `AddGoogleMaps`. See [`dependency-injection.md`](dependency-injection.md).

8. **Docs** — `dotnet format`, add XML doc comments on every public member (the build generates the
   doc file). Don't touch `CHANGELOG.md` (auto-generated); write a conventional-commit subject instead.

## Gotchas

- **Both** `IGoogleMapsClient` and `GoogleMapsClient` must list the property, or the build breaks.
- POST APIs currently each build their own `JsonSerializerOptions` for the body — see the consolidation
  note in [`serialization.md`](serialization.md) and [`known-issues.md`](known-issues.md) (B2).
- Don't add URL signing (`SignableRequest`) to a POST API — Google's new APIs don't support it.
