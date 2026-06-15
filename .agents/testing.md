# Testing

Framework: **NUnit**. Test projects target **`net8.0; net10.0`**. The contributor-facing version of
this is in [`CONTRIBUTING.md`](../CONTRIBUTING.md); this file is the agent quick reference.

## Two tracks

| | Unit tests | Integration tests |
| --- | --- | --- |
| Where | `GoogleMapsApi.Test/*.cs` (root) | `GoogleMapsApi.Test/IntegrationTests/*.cs` |
| Network | **Hermetic** — mock `HttpMessageHandler` (e.g. `CapturingHandler`/`RecordingHandler`) | **VCR** — replay committed cassettes by default; live only in record/auto/live modes |
| Needs key | No | No in `replay` (default); yes in live modes |
| Base class | — | `BaseTestIntegration` |

Examples of unit coverage: `JsonConverterTests`, `GoogleMapsClientTests`, `HttpEngineModernizationTests`,
`EdgeCaseAndErrorHandlingTests`, `DiagnosticsTracingTests`, `NullableReferenceTypesCompatibilityTests`,
`VcrHarnessTests` (hermetic tests for the VCR harness itself), plus the per-API `*UnitTests`. DI
registration is covered in `GoogleMapsApi.Extensions.DependencyInjection.Test`.

## VCR record/replay (resolves former tradeoff B9)

Integration tests are now hermetic by default. The harness lives in `GoogleMapsApi.Test/Vcr/`:
`VcrDelegatingHandler` is inserted into the per-test `HttpClient` pipeline by `BaseTestIntegration`
and reads/writes per-test cassettes under `GoogleMapsApi.Test/Cassettes/<Fixture>/<Test>.json`.

Mode is set by `VCR_MODE` (default `replay`), parsed by `Vcr/VcrMode.cs`:

| `VCR_MODE` | Behavior | Key? | Charges? |
| --- | --- | --- | --- |
| `replay` *(default)* | Serve from cassettes. A **missing cassette file** → test is `Ignore`d (not recorded yet). A request **missing inside an existing** cassette → throws (API drift). | No | No |
| `record` | Hit live Google, (over)write cassettes | Yes | Yes |
| `auto` | Replay on hit, record on miss | Yes (misses) | Misses |
| `live` | Passthrough to Google, no cassettes | Yes | Yes |

Cassettes redact `key`/`signature` (same regex as `MapsAPIGenericEngine`), base64 the body (covers JSON
**and** binary GeoTIFF/photo), and match on `method + redacted URL + normalized JSON body`. In replay the
handler yields and honors the cancellation token so cancellation/timeout plumbing still behaves.

```bash
dotnet test                                                            # replay, offline, no key
VCR_MODE=record GOOGLE_API_KEY=<key> RUN_BILLABLE_TESTS=true dotnet test  # (re)record, then commit cassettes
VCR_MODE=live   GOOGLE_API_KEY=<key> RUN_BILLABLE_TESTS=true dotnet test  # drift-check against live Google
```

`BaseTestIntegration` returns a placeholder key in `replay`; in live modes it resolves the key from
`appsettings.json` (`Utils/AppSettings.cs`) → `GOOGLE_API_KEY` env → throws. Tests set
`request.ApiKey = ApiKey` per call.

> Cancellation/timeout-only tests (e.g. in `GeocodingTests`) make no recordable call, so they have no
> cassette and stay `Ignore`d in `replay`; they execute in live modes.

## Billable tests (gate applies in live modes only)

Some Google APIs (**Places**, **Aerial View**, **Solar**) exceed the free quota. Their fixtures are
tagged `[BillableTest]` (`GoogleMapsApi.Test/Utils/BillableTestAttribute.cs`), which:
- adds the test to the **`Billable`** NUnit category, and
- marks it **Ignored** only when running in a **live mode** (`VcrModes.IsLive`) and `RUN_BILLABLE_TESTS`
  is not truthy. In `replay` it **never** skips — replayed billable tests cost nothing.

```bash
dotnet test                                                  # replay: billable tests RUN (from cassettes)
VCR_MODE=live RUN_BILLABLE_TESTS=true dotnet test            # live: billable tests run against Google
VCR_MODE=live dotnet test --filter "TestCategory=Billable"   # live billable WITHOUT opt-in → skipped
```

When adding a fixture that calls a billable API, **tag it `[BillableTest]`**.

In CI (`.github/workflows/dotnet.yml`) the default push/PR job runs in `replay` (offline, covers the
whole suite incl. billable). A separate scheduled/dispatch job runs live with the key +
`RUN_BILLABLE_TESTS` to detect drift; the `run-billable-tests` PR label also triggers a live run.

## Quota handling

`Utils/AssertInconclusive` converts an `OVER_QUERY_LIMIT` response into an NUnit **inconclusive**
result instead of a hard failure, so a quota blip doesn't read as a code regression. (Trade-off: an
inconclusive result is easy to overlook in CI output.)

## Conventions

- **Test naming:** descriptive, behavior-first. Both `Subject_Scenario_Outcome` (e.g.
  `DurationJsonConverter_Directions_DeserializesCorrectly`) and `Should…` styles exist; prefer
  `Subject_Scenario_Outcome` for new tests and stay consistent within a fixture.
- The library exposes internals to the test project via `InternalsVisibleTo("GoogleMapsApi.Test")`,
  so you can unit-test internal engine types directly.
- When fixing a reported bug, add a failing regression test first, then make it pass.

## Commands

```bash
dotnet test                                          # all frameworks, billable skipped
dotnet test --framework net10.0                      # single TFM
dotnet test --filter "ClassName=DirectionsTests"     # one fixture
dotnet test --collect:"XPlat Code Coverage"          # coverage
```
