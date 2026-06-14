# Observability, events & error handling

Tracing, the two diagnostic events, and how failures surface. All of this lives in
`GoogleMapsApi/Engine/MapsAPIGenericEngine.cs` and `GoogleMapsApi/Diagnostics/GoogleMapsActivity.cs`.

## OpenTelemetry tracing

The library emits **one `Activity` (span) per API call** from an `ActivitySource`:

- Source name: **`GoogleMapsActivity.SourceName == "GoogleMapsApi"`** (versioned from the assembly).
- Span name: `GoogleMapsApi {Api}`, where `{Api}` is the request type minus the `Request` suffix
  (e.g. `GoogleMapsApi Geocoding`). Kind = `Client`.
- Instrumentation is **zero-cost until a listener subscribes** (`StartActivity` returns null, all
  tagging is null-guarded). It is always on; you opt in by collecting it.

Consumer wiring (OpenTelemetry):

```csharp
tracerProviderBuilder.AddSource(GoogleMapsApi.Diagnostics.GoogleMapsActivity.SourceName);
```

### Span tags

Follows OTel HTTP semantic conventions plus a `gmaps.*` namespace:

| Tag | Value |
| --- | --- |
| `gmaps.api` | service name (e.g. `Geocoding`) |
| `http.request.method` | `GET` or `POST` (POST when the request has a body) |
| `server.address` | request host |
| `url.full` | **redacted** full URL — `key=` and `signature=` values replaced with `REDACTED` |
| `http.response.status_code` | HTTP status |
| `gmaps.response_status` | the Google `Status` field, if the response type has one |
| `error.type` | exception type full name (on failure) |

On exception the span is set to `ActivityStatusCode.Error` with the message and rethrown.

> **Known issue (B4):** `http.response.status_code` is set via `Activity.Current` inside
> `GetHttpResponseAsync` rather than the captured `activity` reference (`MapsAPIGenericEngine.cs:122`).
> Fine in the common path; can misattribute under unusual ambient-Activity nesting. See
> [`known-issues.md`](known-issues.md).
>
> **Gaps (B5):** tracing only — there is no `ILogger` integration and no metrics (counters/histograms).
> Latency is implicit in span duration. Logging is a candidate enhancement.

## The two events (`IEngineFacade`)

Per-instance, synchronous (see [`architecture.md`](architecture.md)):

- `OnUriCreated(Uri) → Uri` — fired before the HTTP call; return a modified `Uri` to rewrite it.
- `OnRawResponseReceived(byte[])` — fired with the **raw UTF-8 response body** before deserialization.

### Security note (B6)

`OnRawResponseReceived` hands callers the **unredacted** response bytes — which can contain addresses,
coordinates, place names, and other PII. The `url.full` redaction does **not** apply here. If you log,
cache, or forward this payload, sanitize it yourself. Don't wire it to a sink that ships raw bodies to
third parties without review.

## Error model — two separate channels

1. **Transport/HTTP failures throw** (`MapsAPIGenericEngine.HandleHttpResponse`):
   - `401` / `403` / `407` → `System.Security.Authentication.AuthenticationException`
   - `408` / `504` → `TimeoutException`
   - any other non-2xx → `HttpRequestException`
   - client timeout → `TimeoutException`; caller cancellation → `OperationCanceledException`
     (see the timeout section in [`architecture.md`](architecture.md)).

2. **Google business errors do NOT throw.** A `200 OK` whose body says
   `status: "ZERO_RESULTS" | "OVER_QUERY_LIMIT" | "REQUEST_DENIED" | …` deserializes normally —
   **callers must check `response.Status`**. The span's `gmaps.response_status` reflects it, but the
   span status stays `Unset` (not `Error`). Tests use `AssertInconclusive` to treat `OVER_QUERY_LIMIT`
   as inconclusive rather than a failure (see [`testing.md`](testing.md)).
