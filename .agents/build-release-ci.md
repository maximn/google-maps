# Build, versioning, release & CI/CD

Procedure-level detail for humans is in [`RELEASING.md`](../RELEASING.md); this is the agent map.

## Target frameworks

- **Core `GoogleMapsApi`:** `netstandard2.0; net8.0; net10.0` (`GoogleMapsApi/GoogleMapsApi.csproj`).
  `netstandard2.0` is what covers .NET Framework / older runtimes — there are **no** `net481`/`net462`
  TFMs anymore (dropped in 2.0, along with `net6.0`).
- **DI extension:** mirrors the same three TFMs.
- **Templates package `GoogleMapsApi.Templates`** (`dotnet new` template, ships the `googlemaps-webapi`
  template): a `PackageType=Template` package — no build output (`IncludeBuildOutput=false`,
  `EnableDefaultCompileItems=false`), so its single `netstandard2.0` TFM is metadata-only. Its
  `templates/` tree packs under `content/`. A `BeforeTargets="_GetPackageFiles"` target stamps the
  resolved MinVer version into the packed `template.json`, so generated projects reference the
  matching `GoogleMapsApi` version.
- **Tests:** `net8.0; net10.0`. **Samples:** `net10.0; net8.0`.
- `LangVersion=latest`, `Nullable=enable`, `GenerateDocumentationFile=true` (CS1591 suppressed).

> SDK install lists in the workflows (e.g. `6.0.x`) are *SDKs*, not target frameworks — a newer SDK
> builds older TFMs. Don't infer TFMs from `setup-dotnet`.

## Versioning — MinVer, tag-driven

Versions come from **git tags** via **MinVer** (`<MinVerTagPrefix>v</MinVerTagPrefix>`). No version is
written in a csproj. `GeneratePackageOnBuild=true`, SourceLink + symbol packages (`.snupkg`) are on.
All three packages (`GoogleMapsApi`, `…Extensions.DependencyInjection`, `…Templates`) are versioned in
**lockstep** from the same `v*` tag. Adding a packable project to `GoogleMapsApi.sln` is enough for the
publish workflow to pack and push it — it globs `*.nupkg`, no per-package list.

## Release flow (`release.sh`)

```bash
./release.sh [patch|minor|major] [--dry-run] [--no-notes] [--edit]
```

It: computes the next version → generates Keep-a-Changelog notes with `claude -p` from the commit log →
rewrites `CHANGELOG.md` (moves `[Unreleased]` into the new version section) → commits → creates an
**annotated `v*` tag** → pushes → `gh release create`.

- **`CHANGELOG.md` is auto-generated — never hand-edit it in a feature PR.** Write a good
  conventional-commit subject; that becomes the changelog line.
- `release.sh` needs the `claude` and `gh` CLIs on PATH (or use `--no-notes`).

## Publishing (`.github/workflows/nuget.yml`)

Triggered by **pushing a `v*` tag** (or manual `workflow_dispatch` with a `version` + `dry_run` input).
It checks out full history (`fetch-depth: 0`, MinVer needs tags), builds/packs Release, generates a
per-package **CycloneDX SBOM** (`*.bom.json`, via the `dotnet CycloneDX` local tool pinned in
`.config/dotnet-tools.json`) from temporary consumers that restore the freshly packed local packages,
creates **SLSA build-provenance attestations**, pushes `.nupkg`+`.snupkg` to NuGet.org with
`--skip-duplicate`, and attaches the packages, SBOMs, and attestation bundle to the GitHub Release.

The publish job runs in the **`release` GitHub Environment** (a required reviewer must approve the
deployment before it runs), and `NUGET_API_KEY` is a `release` **environment** secret — so only the
gated job can read it. `workflow_dispatch` **dry-runs stay ungated** (the job's `environment:`
expression resolves to no environment when `dry_run` is left at its `true` default), so they pack +
SBOM + attest without prompting. A real `v*` tag push or a `dry_run=false` manual run pauses on
"Review deployments" until approved.

> **OPS note:** approving the `release` deployment is the point of no return — `--skip-duplicate`
> means a same-version re-run is a no-op, but a published version cannot be unpublished from NuGet.org.

### One-time `release` environment setup

Repo-admin steps (recorded here so they're reproducible). `prevent_self_review=false` lets a solo
maintainer approve their own release (set `true` and use a distinct reviewer for a team). No
deployment-branch policy is set: the reviewer gate already governs every deployment, and leaving it
open keeps both the tag-push and the manual `workflow_dispatch` publish (from `master`) working.

```bash
REVIEWER_ID=$(gh api users/maximn --jq .id)
gh api --method PUT repos/maximn/google-maps/environments/release --input - <<JSON
{
  "wait_timer": 0,
  "prevent_self_review": false,
  "reviewers": [{ "type": "User", "id": $REVIEWER_ID }],
  "deployment_branch_policy": null
}
JSON

# Scope the publish key to the gated job: add it under the environment, then drop the repo copy.
gh secret set NUGET_API_KEY --env release --repo maximn/google-maps   # prompts for value
gh secret delete NUGET_API_KEY --repo maximn/google-maps              # only after a gated publish succeeds
```

## CI (`.github/workflows/dotnet.yml`)

Runs on push to `master`, PRs (including the `labeled` event), and manual dispatch. Builds all TFMs;
the default push/PR run **tests on `net10.0` only** — use `workflow_dispatch` (`frameworks` input) to
run `net8.0` or both. Uses the `GOOGLE_API_KEY` secret for live integration tests. Billable Places tests
are gated by the `run-billable-tests` label or the dispatch input (see [`testing.md`](testing.md)).

Two heavy jobs never gate PRs — both `workflow_dispatch`, and run on demand or on a schedule:
`benchmarks.yml` (BenchmarkDotNet) and `mutation.yml` (Stryker.NET mutation testing, weekly Monday
07:00 UTC — see [`testing.md`](testing.md)).

## Security & dependencies

- `codeql.yml` — CodeQL scanning (scheduled + push/PR). `scorecard.yml` — OpenSSF Scorecard
  supply-chain checks.
- `renovate.json` — auto-merges non-major GitHub Actions and test/dev dependency updates and patch
  updates for runtime deps, with a 3-day `minimumReleaseAge`.

> **OPS note:** Renovate auto-merge relies on branch protection requiring CI. Verified on `master`
> (2026-06-29): `required_status_checks` enforces `build` + `Analyze (csharp)` + `Analyze (actions)`,
> so a non-major update can't auto-merge without a green build + CodeQL.

## Commands

```bash
dotnet restore
dotnet build -c Release
dotnet pack --no-build               # .nupkg + .snupkg
dotnet format --verify-no-changes    # CI-style format check
```

> Note: packaging is entirely csproj-driven via `GeneratePackageOnBuild` — there is no `.nuspec`.
