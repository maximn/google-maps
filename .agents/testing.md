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
| NUnit category | — | `Integration` (inherited from the base class) |

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
| `replay` *(default)* | Serve from cassettes. A missing cassette or request fails loudly so integration coverage cannot silently disappear. | No | No |
| `record` | Hit live Google and replace each test's cassette | Yes | Yes |
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

> Cancellation/timeout-only tests (e.g. in `GeocodingTests`) need no cassette: replay observes
> cancellation before attempting cassette matching.

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

In CI (`.github/workflows/dotnet.yml`) the default push/PR job always runs unit tests and the VCR
harness offline. It also runs the `Integration` category in replay once cassette JSON files are
committed; until then it emits an explicit warning. A separate scheduled/dispatch job runs the
`Integration` category live with the key + `RUN_BILLABLE_TESTS` to detect drift; the
`run-billable-tests` PR label also triggers a live run.

## Mutation testing (Stryker.NET)

Line coverage proves code *ran*; mutation testing proves the assertions actually *catch*
regressions. [Stryker.NET](https://stryker-mutator.io/docs/stryker-net/introduction/) injects faults
into the shipped `GoogleMapsApi` library and checks the suite fails. Surviving mutants flag behavior
no test pins down.

It's slow, so it never gates PRs — `.github/workflows/mutation.yml` runs it weekly (Monday 07:00
UTC) and on `workflow_dispatch`. The job publishes the score to the run's job summary and uploads the
HTML report as the `mutation-report` artifact. The `break` threshold in `stryker-config.json` fails
the job below that score. Baseline at introduction was **~46%**, so `break` is set to **40** (a little
headroom); ratchet it up as test assertions improve rather than letting the score drift down.

Run it locally (offline, from cassettes — no key, no charges):

```bash
dotnet tool restore                                  # installs pinned dotnet-stryker (.config/dotnet-tools.json)
cd GoogleMapsApi.Test && VCR_MODE=replay dotnet stryker
```

Config lives in `GoogleMapsApi.Test/stryker-config.json` (mutates core only, target `net10.0`).

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
