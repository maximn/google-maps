# Architecture

How the library is structured and how a single API call flows from your code to a deserialized
response. For *adding* an API, see [`adding-a-new-api.md`](adding-a-new-api.md).

## The layers

```
your code
   │  new GoogleMapsClient(httpClient[, options])     // public, instance-based
   ▼
GoogleMapsClient : IGoogleMapsClient                  // GoogleMapsApi/GoogleMapsClient.cs
   │  exposes 12 service properties, each an
   │  IEngineFacade<TRequest,TResponse>               // GoogleMapsApi/IEngineFacade.cs (public)
   ▼
HttpClientEngineFacade<TRequest,TResponse>            // GoogleMapsApi/Engine/HttpClientEngineFacade.cs (internal)
   │  holds the HttpClient + GoogleMapsClientOptions
   │  auto-fills the ambient ApiKey, owns the events
   ▼
MapsAPIGenericEngine<TRequest,TResponse>              // GoogleMapsApi/Engine/MapsAPIGenericEngine.cs (internal, static)
   │  builds the URI / body, starts the trace span,
   │  GETs or POSTs, maps HTTP errors, deserializes JSON
   ▼
Google Maps Web Service  ──►  TResponse (IResponseFor<TRequest>)
```

There is **no static facade** — the static `GoogleMaps` entry point and the legacy Places API were
removed in 2.0 (see [`MIGRATION.md`](../MIGRATION.md)). `GoogleMapsClient` is `sealed`; the two engine
types are `internal`. The only public moving parts you implement against are `IGoogleMapsClient`,
`IEngineFacade<,>`, `MapsBaseRequest`, and `IResponseFor<>`.

## The request/response contract

- **Requests** derive from `MapsBaseRequest` (`Entities/Common/MapsBaseRequest.cs`):
  - `IsSSL` defaults to `true`; `ApiKey` is nullable.
  - `GetUri()` composes `scheme + BaseUrl + "json?" + query`. Each request overrides `BaseUrl` to add
    its service path (e.g. `GeocodingRequest` adds `geocode/`).
  - `GetQueryStringParameters()` builds the GET query (via `QueryStringParametersList`, which
    URL-escapes and **skips null values**).
  - `GetRequestBody()` returns `null` for GET endpoints, or an `HttpContent` JSON body for POST
    endpoints. **A non-null body is how the engine decides to POST instead of GET.**
  - Setting an `ApiKey` while `IsSSL == false` throws `ArgumentException` (keys must go over HTTPS).
- **Responses** implement the marker interface `IResponseFor<TRequest>` (`Entities/Common/IResponseFor.cs`).
- The engine is constrained `where TRequest : MapsBaseRequest, new()` and
  `where TResponse : IResponseFor<TRequest>`, so a request can only be paired with its own response —
  mismatches are compile errors.

Two request base classes exist — see [`api-catalog.md`](api-catalog.md):
- **`SignableRequest : MapsBaseRequest`** — legacy GET APIs; adds `ClientID`/`SigningKey`/`Channel` for
  Google Enterprise URL signing (HMAC-SHA1).
- **`MapsBaseRequest`** directly — the new POST APIs (Routes, Address Validation, Places New); these
  authenticate with an API key only (no URL signing).

## Ambient API key

`GoogleMapsClientOptions.ApiKey` is an *ambient* default. In `HttpClientEngineFacade.QueryAsync`, if
`request.ApiKey` is empty **and** the options have a key, the request is shallow-cloned
(`MapsBaseRequest.CloneShallow()` → `MemberwiseClone`) and the key is filled into the copy — the
caller's request object is never mutated. A key set on the request always wins.

> Gotcha: `CloneShallow()` is a shallow copy. It's correct for the scalar `ApiKey`, but if you add
> mutable reference fields to a request, the clone shares them.

## Timeout and cancellation

`MapsAPIGenericEngine.GetHttpResponseAsync` creates a linked `CancellationTokenSource` from the
caller's token and `CancelAfter(timeout)` (the default timeout is `Timeout.Infinite`). The three
outcomes are distinguished deliberately:
- caller's token fired → `OperationCanceledException`
- the timeout fired → `TimeoutException` (with a message naming the limit)
- HTTP `408`/`504` from Google → `TimeoutException`

See [`observability.md`](observability.md) for the full HTTP-error → exception mapping and the
Google-`Status`-field model (business errors are **not** exceptions).

## Per-instance events

`IEngineFacade` exposes two synchronous events, scoped to that facade instance (not global):
- `OnUriCreated(Uri) → Uri` — inspect/rewrite the URL before the request is sent.
- `OnRawResponseReceived(byte[])` — the raw UTF-8 response body, before deserialization.

Because each service property is its own facade, handlers attached to `client.Directions` do not fire
for `client.Geocode`. Keep handlers lightweight (they run inline on the async path). **Security:**
`OnRawResponseReceived` is unredacted and may contain PII — see [`observability.md`](observability.md).

## Key files

| File | Role |
| --- | --- |
| `GoogleMapsApi/GoogleMapsClient.cs` | Public instance client; constructs the 12 service facades |
| `GoogleMapsApi/IGoogleMapsClient.cs` | The public contract (12 service properties) |
| `GoogleMapsApi/IEngineFacade.cs` | The 4 `QueryAsync` overloads + the two events |
| `GoogleMapsApi/GoogleMapsClientOptions.cs` | Ambient options (just `ApiKey` today) |
| `GoogleMapsApi/Engine/HttpClientEngineFacade.cs` | Per-instance facade; ambient-key fill; event wiring |
| `GoogleMapsApi/Engine/MapsAPIGenericEngine.cs` | Static HTTP+JSON engine; tracing; error mapping |
| `GoogleMapsApi/Entities/Common/MapsBaseRequest.cs` | Request base: URI/body composition, SSL+key rules |
| `GoogleMapsApi/Entities/Common/SignableRequest.cs` | URL-signing base for legacy GET APIs |
| `GoogleMapsApi/Entities/Common/IResponseFor.cs` | Request↔response marker interface |
