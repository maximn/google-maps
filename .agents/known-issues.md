# Known issues & tech debt

A curated log of things that are against best practice or worth improving, found while building this
knowledge base. **No fixes were applied here** — this was a documentation pass. Pick these up in
focused follow-up PRs. Each item has a file reference and a recommended fix; severities are the
author's estimate.

When you fix one, remove it from this list (and update the relevant `.agents/*.md` that references it).

| ID | Severity | Issue | Where | Recommended fix |
| --- | --- | --- | --- | --- |
| **B3** | medium | `DurationJsonConverter` and `OverviewPolylineJsonConverter` resolve members by name via reflection (`GetProperty`) — rename-fragile, and they silently accept partial objects (a missing token leaves that field unset). | `Engine/JsonConverters/DurationJsonConverter.cs`, `Engine/JsonConverters/OverviewPolylineJsonConverter.cs` | Replace reflection with direct (de)serialization of explicit DTOs. |
| **B4** | low | HTTP status tag is set on `Activity.Current` instead of the captured `activity`, so it can misattribute under unusual ambient-Activity nesting. | `Engine/MapsAPIGenericEngine.cs:122` | Capture `activity` and tag it directly; pass it down or set the tag inside the parent scope. See [`observability.md`](observability.md). |
| **B7** | low | Inconsistent SSL enforcement (`TimeZoneRequest` forces HTTPS, others don't) and mixed enum handling (some `[EnumMember]`, some bare names). | `Entities/TimeZone/Request/TimeZoneRequest.cs`; various `Entities/**/Response/Status*.cs` | Decide one SSL policy; standardize on `[EnumMember]` (or document when it's intentionally omitted). |

## Documented elsewhere (not defects — cross-references)

- **B6** — `OnRawResponseReceived` exposes unredacted response bytes (potential PII). Documented as a
  consumer **security note** in [`observability.md`](observability.md).
- **B9** — the VCR record/replay harness exists (#306) but **no cassettes are committed yet**, so
  replay integration coverage is dormant: CI's replay step is cassette-guarded and skips, and a local
  `replay` run fails on the integration suite until it's seeded. **Fix:** record + commit the dataset
  (`VCR_MODE=record GOOGLE_API_KEY=<key> RUN_BILLABLE_TESTS=true dotnet test`) — billable; tracked by
  issue #300. See [`testing.md`](testing.md).
