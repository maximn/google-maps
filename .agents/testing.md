# Testing

Framework: **NUnit**. Test projects target **`net8.0; net10.0`**. The contributor-facing version of
this is in [`CONTRIBUTING.md`](../CONTRIBUTING.md); this file is the agent quick reference.

## Two tracks

| | Unit tests | Integration tests |
| --- | --- | --- |
| Where | `GoogleMapsApi.Test/*.cs` (root) | `GoogleMapsApi.Test/IntegrationTests/*.cs` |
| Network | **Hermetic** — mock `HttpMessageHandler` (e.g. `CapturingHandler`/`RecordingHandler`) | **Live Google APIs** — real HTTP, counts against quota |
| Needs key | No | Yes (`GOOGLE_API_KEY`) |
| Base class | — | `BaseTestIntegration` |

Examples of unit coverage: `JsonConverterTests`, `GoogleMapsClientTests`, `HttpEngineModernizationTests`,
`EdgeCaseAndErrorHandlingTests`, `DiagnosticsTracingTests`, `NullableReferenceTypesCompatibilityTests`,
plus the per-API `*UnitTests`. DI registration is covered in
`GoogleMapsApi.Extensions.DependencyInjection.Test`.

> **Deliberate tradeoff (B9):** integration tests are **not hermetic** — there is no VCR/cassette
> replay. They hit live Google endpoints, so they need a real key, a network, and available quota, and
> can fail for reasons unrelated to your change. New *unit* tests should stay hermetic with a mocked
> handler; reserve live calls for the integration suite.

## API key for integration tests

`BaseTestIntegration` resolves the key in this order (`IntegrationTests/BaseTestIntegration.cs`):
1. `appsettings.json` (`GOOGLE_API_KEY` property) loaded from the test output dir by `Utils/AppSettings.cs`
2. the `GOOGLE_API_KEY` environment variable
3. otherwise throws `InvalidOperationException`.

Local setup: copy `GoogleMapsApi.Test/appsettings.template.json` → `appsettings.json` and fill it in,
**or** export `GOOGLE_API_KEY`. The base class shares one static `HttpClient` and one
`GoogleMapsClient(SharedHttpClient)`; tests set `request.ApiKey = ApiKey` per call.

## Billable tests (off by default)

Some Google APIs (currently **Places**) exceed the free quota. Their fixtures are tagged
`[BillableTest]` (`GoogleMapsApi.Test/Utils/BillableTestAttribute.cs`), which:
- adds the test to the **`Billable`** NUnit category, and
- marks it **Ignored** unless `RUN_BILLABLE_TESTS` is truthy (`1`, `true`, or `yes`, case-insensitive).

```bash
dotnet test                                                  # billable tests skipped
RUN_BILLABLE_TESTS=true dotnet test                          # include them (incurs charges)
RUN_BILLABLE_TESTS=true dotnet test --filter "TestCategory=Billable"   # only them
```

When adding a fixture that calls a billable API, **tag it `[BillableTest]`** so it stays off by default.

In CI (`.github/workflows/dotnet.yml`) billable tests are skipped on ordinary push/PR. Run them on
demand by adding the **`run-billable-tests`** label to a PR (collaborators only) or via the manual
**Run workflow** dispatch input.

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
