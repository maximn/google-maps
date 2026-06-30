# `.agents/` — agent knowledge base

This directory is the durable, agent-facing knowledge base for the GoogleMapsApi repo. It exists so
that any coding agent (or human) starts a session with an accurate mental model and can jump straight
to the one document relevant to the task — instead of re-deriving the architecture from source or
trusting stale prose.

**Conventions for these files**
- Each file is **focused on one concern** and kept concise. Load only what the task needs.
- Claims are **grounded in real code** with `path` or `path:line` references. If you change behavior,
  update the matching `.agents/*.md` in the same PR.
- These files **link to the authoritative human docs** ([`README.md`](../README.md),
  [`MIGRATION.md`](../MIGRATION.md), [`RELEASING.md`](../RELEASING.md),
  [`CONTRIBUTING.md`](../CONTRIBUTING.md)) rather than duplicating them.
- The lean entry point with the most-used commands and the routing table is [`../CLAUDE.md`](../CLAUDE.md).

## Files

| File | Read it when you need to… | Key references |
| --- | --- | --- |
| [`architecture.md`](architecture.md) | Understand the layers and how one request flows end to end | `GoogleMapsClient`, `MapsAPIGenericEngine` |
| [`adding-a-new-api.md`](adding-a-new-api.md) | **Add a new Google API or extend an existing one** (the main recipe) | `Entities/`, `IGoogleMapsClient` |
| [`api-catalog.md`](api-catalog.md) | See which APIs exist, the legacy→new map, and GET vs POST | `IGoogleMapsClient`, `Entities/` |
| [`serialization.md`](serialization.md) | Work with JSON, custom converters, or enums | `Engine/JsonConverters/` |
| [`observability.md`](observability.md) | Touch tracing, the request/response events, or error handling | `Diagnostics/`, `MapsAPIGenericEngine` |
| [`testing.md`](testing.md) | Write or run tests, or deal with billable/live-API gating | `GoogleMapsApi.Test/` |
| [`dependency-injection.md`](dependency-injection.md) | Use/extend `AddGoogleMaps`, DI, or the samples | `GoogleMapsApi.Extensions.DependencyInjection/` |
| [`build-release-ci.md`](build-release-ci.md) | Build, version, release, or change CI/CD | `*.csproj`, `release.sh`, `.github/workflows/` |
| [`public-api.md`](public-api.md) | Change the public surface (this can break the build) | `PublicAPI.*.txt`, `.editorconfig` |
| [`conventions.md`](conventions.md) | Match code style and commit conventions | `.editorconfig` |
| [`known-issues.md`](known-issues.md) | Pick up known tech debt / refactor candidates | — |

## Fast facts (the things people get wrong)

- **No static facade.** The entry point is `new GoogleMapsClient(httpClient)` / `IGoogleMapsClient`.
  Anything describing `GoogleMaps.Geocode...` is pre-2.0 and wrong.
- **Target frameworks:** `netstandard2.0; net8.0; net10.0` (the core library). Not 6 frameworks —
  `net6.0`, `net481`, `net462` were dropped in 2.0.
- **Serialization is `System.Text.Json`**, not Newtonsoft.
- **Integration tests default to VCR replay.** `VCR_MODE=replay` (the default) serves committed
  cassettes offline — no `GOOGLE_API_KEY`, no charges. Only the opt-in `record`/`auto`/`live` modes
  need a real key. (Cassettes aren't seeded yet, so replay is dormant until recorded — see
  [`testing.md`](testing.md).)
