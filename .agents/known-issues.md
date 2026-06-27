# Known issues & tech debt

A curated log of things that are against best practice or worth improving, found while building this
knowledge base. **No fixes were applied here** — this was a documentation pass. Pick these up in
focused follow-up PRs. Each item has a file reference and a recommended fix; severities are the
author's estimate.

When you fix one, remove it from this list (and update the relevant `.agents/*.md` that references it).

| ID | Severity | Issue | Where | Recommended fix |
| --- | --- | --- | --- | --- |
| **B2** | medium | POST APIs each build their own `new JsonSerializerOptions` for the request body, duplicating setup and risking drift from the shared response options. | `Entities/Routes/Request/RoutesRequest.cs`, `Entities/AddressValidation/Request/AddressValidationRequest.cs`, `Entities/PlacesNew/Request/{SearchText,SearchNearby,Autocomplete}Request.cs` | Factor body options into a shared helper (mirror `Engine/JsonSerializerConfiguration.cs`). See [`serialization.md`](serialization.md). |
| **B3** | medium | `DurationJsonConverter` and `OverviewPolylineJsonConverter` resolve members by name via reflection (`GetProperty`) — rename-fragile, and they silently accept partial objects (a missing token leaves that field unset). | `Engine/JsonConverters/DurationJsonConverter.cs`, `Engine/JsonConverters/OverviewPolylineJsonConverter.cs` | Replace reflection with direct (de)serialization of explicit DTOs. |
| **B4** | low | HTTP status tag is set on `Activity.Current` instead of the captured `activity`, so it can misattribute under unusual ambient-Activity nesting. | `Engine/MapsAPIGenericEngine.cs:122` | Capture `activity` and tag it directly; pass it down or set the tag inside the parent scope. See [`observability.md`](observability.md). |
| **B5** | low | Observability is tracing-only — no `ILogger` integration and no metrics (counters/histograms). | `Engine/MapsAPIGenericEngine.cs` | Optional: add an `ILogger`-based debug log and OTel metrics (request count, duration histogram, error count). |
| **B7** | low | Inconsistent SSL enforcement (`TimeZoneRequest` forces HTTPS, others don't) and mixed enum handling (some `[EnumMember]`, some bare names). | `Entities/TimeZone/Request/TimeZoneRequest.cs`; various `Entities/**/Response/Status*.cs` | Decide one SSL policy; standardize on `[EnumMember]` (or document when it's intentionally omitted). |
| **B8** | medium | No manual approval gate before NuGet publish (a `v*` tag push publishes immediately), and Renovate auto-merge assumes branch protection is configured. | `.github/workflows/nuget.yml`, `renovate.json` | Confirm `master` branch protection requires CI; optionally add a release approval environment. (Process decision for the maintainer.) See [`build-release-ci.md`](build-release-ci.md). |

## Documented elsewhere (not defects — cross-references)

- **B6** — `OnRawResponseReceived` exposes unredacted response bytes (potential PII). Documented as a
  consumer **security note** in [`observability.md`](observability.md).
- **B9** — integration tests hit live Google APIs with no VCR/cassette (non-hermetic). This is a
  **deliberate** project decision; documented as a tradeoff in [`testing.md`](testing.md).
