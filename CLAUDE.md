# CLAUDE.md

Guidance for AI agents (Claude Code & others) working in this repository. Keep this file lean —
deep knowledge lives in [`.agents/`](.agents/README.md). Read the routing table below and open the
one file relevant to your task.

## What this is

A strongly-typed .NET wrapper for the Google Maps Web Services APIs (Geocoding, Directions/Routes,
Distance Matrix, Elevation, Time Zone, Places (New), Address Validation, Static Maps). Shipped on
NuGet as `GoogleMapsApi` (+ siblings `GoogleMapsApi.Extensions.DependencyInjection` and the
`dotnet new` template pack `GoogleMapsApi.Templates`).

## Architecture in 30 seconds

- **Entry point is the instance-based `GoogleMapsClient` / `IGoogleMapsClient`** — you construct it
  with an injected `HttpClient`. The old **static `GoogleMaps` facade was removed in 2.0** (so was the
  legacy Places API). Do not reintroduce static state.
- **Async-only.** Every call goes through `IEngineFacade<TRequest,TResponse>.QueryAsync(...)`.
- **Request/response pattern:** requests derive `MapsBaseRequest` (override `GetUri()` for GET or
  `GetRequestBody()` for POST); responses implement `IResponseFor<TRequest>`. Generic constraints make
  mismatches a compile error.
- **Observability is built in:** one OpenTelemetry span per call from `ActivitySource "GoogleMapsApi"`,
  with the API key/signature redacted from the traced URL.
- **Three packages, lockstep-versioned:** core `GoogleMapsApi`, the optional DI extension, and the
  `dotnet new` template pack `GoogleMapsApi.Templates`.

Flow: `GoogleMapsClient` → `IEngineFacade` → `HttpClientEngineFacade` (internal) → `MapsAPIGenericEngine` (internal abstract; static-method HTTP+JSON engine).

## Commands used in most sessions

```bash
dotnet restore
dotnet build                       # TFMs: netstandard2.0; net8.0; net10.0
dotnet test                        # billable (Places) tests are SKIPPED by default
dotnet format                      # run before committing — enforces .editorconfig
dotnet pack --no-build             # produces .nupkg + .snupkg

RUN_BILLABLE_TESTS=true dotnet test    # opt in to billable Places tests (incurs charges)
dotnet test --filter "ClassName=DirectionsTests"      # one fixture
```

## Rules that break the build or release if ignored

- **Public API is locked.** Any change to the public surface must be reflected in
  `GoogleMapsApi/PublicAPI.Unshipped.txt` or the build fails (`RS00xx` are errors). → [`.agents/public-api.md`](.agents/public-api.md)
- **`CHANGELOG.md` is auto-generated** by `release.sh` from commit messages — **never hand-edit it**
  in a feature PR. → [`RELEASING.md`](RELEASING.md)
- **Conventional commits** (`feat:`, `fix:`, `chore:` …) — they drive the changelog and version bump.
- **`dotnet format`** before committing; public members need XML doc comments.

## Where to look (routing)

| If you're working on… | Read |
| --- | --- |
| The big picture / how a request flows | [`.agents/architecture.md`](.agents/architecture.md) |
| **Adding or extending an API** | [`.agents/adding-a-new-api.md`](.agents/adding-a-new-api.md) |
| Which APIs exist, legacy→new map, GET vs POST | [`.agents/api-catalog.md`](.agents/api-catalog.md) |
| JSON, custom converters, enums | [`.agents/serialization.md`](.agents/serialization.md) |
| Tracing, events, error model | [`.agents/observability.md`](.agents/observability.md) |
| Writing/running tests, billable gating | [`.agents/testing.md`](.agents/testing.md) |
| Dependency injection & samples | [`.agents/dependency-injection.md`](.agents/dependency-injection.md) |
| Build, versioning, release, CI/CD | [`.agents/build-release-ci.md`](.agents/build-release-ci.md) |
| The public-API lock workflow | [`.agents/public-api.md`](.agents/public-api.md) |
| Code style & commit conventions | [`.agents/conventions.md`](.agents/conventions.md) |
| Known tech debt / refactor candidates | [`.agents/known-issues.md`](.agents/known-issues.md) |

Start here for the index: [`.agents/README.md`](.agents/README.md).

## Canonical human docs (don't duplicate — link)

[`README.md`](README.md) (usage, published as the NuGet readme) · [`MIGRATION.md`](MIGRATION.md) (1.x→2.0) ·
[`CONTRIBUTING.md`](CONTRIBUTING.md) · [`RELEASING.md`](RELEASING.md).
