# Releasing

This document describes how to publish a new version of the `GoogleMapsApi` NuGet package.

Releases are **tag-driven**: pushing a git tag matching `v*` to `origin` triggers the [`Publish Nuget`](.github/workflows/nuget.yml) workflow, which builds the package and pushes it to [NuGet.org](https://www.nuget.org/packages/GoogleMapsApi/).

## Prerequisites

- Push access to `origin`
- A clean working tree on the latest `master`:
  ```bash
  git checkout master
  git pull
  git status   # should be clean
  ```
- The `NUGET_API_KEY` secret is configured as a `release` **environment** secret (one-time setup — see [`.agents/build-release-ci.md`](.agents/build-release-ci.md))
- You can approve the `release` environment deployment (the publish run pauses for a required reviewer)

## Option 1: Use the release script (recommended)

`release.sh` reads the latest `v*` tag, computes the next version, and pushes the tag for you.

```bash
./release.sh           # patch bump (e.g. 1.4.5 → 1.4.6)
./release.sh minor     # minor bump (e.g. 1.4.5 → 1.5.0)
./release.sh major     # major bump (e.g. 1.4.5 → 2.0.0)
```

Preview without making changes:

```bash
./release.sh --dry-run
./release.sh minor --dry-run
```

The script:
1. Verifies the working tree is clean
2. Reads the latest `v*` tag
3. Computes the next version based on the bump type (defaults to `patch`)
4. Generates release notes from the commit log via `claude -p`, using [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) categories (`Added` / `Changed` / `Deprecated` / `Removed` / `Fixed` / `Security`)
5. Shows you the notes and prompts `y/N` before doing anything (`--edit` opens them in `$EDITOR` first)
6. On confirmation:
   - Moves `## [Unreleased]` content into a new `## [NEW_VERSION] - YYYY-MM-DD` section in [`CHANGELOG.md`](CHANGELOG.md) and refreshes the compare-link footnotes
   - Commits `CHANGELOG.md` as `chore(release): vX.Y.Z` and pushes it to the current branch
   - Creates the annotated tag (with the notes as the tag message) and pushes it to `origin`
   - Creates a GitHub Release with the same notes

Use `--no-notes` to skip the `claude -p` step, the `CHANGELOG.md` update, and the GitHub Release — useful for out-of-band hotfix tags where you just want to push the `v*` tag.

## Option 2: Tag manually

If you want to pick an explicit version (out-of-band release, hotfix on an older line, etc.):

```bash
git tag v1.4.6
git push origin v1.4.6
```

If you go this route, update [`CHANGELOG.md`](CHANGELOG.md) by hand: move the `## [Unreleased]` content into a new `## [1.4.6] - YYYY-MM-DD` section and add a matching `[1.4.6]: …/compare/v1.4.5...v1.4.6` footnote.

Both options end at the same place — a `v*` tag in `origin` — and trigger the same workflow.

## Choosing the bump type (semver)

- **patch** — bug fixes, dependency bumps, internal cleanup; no public API change
- **minor** — new functionality, backwards-compatible additions
- **major** — breaking changes to the public API

## What the workflow does

[`.github/workflows/nuget.yml`](.github/workflows/nuget.yml) runs on `push` of any `v*` tag:

0. **Waits for approval** — the publish job runs in the `release` environment, so the run pauses on
   "Review deployments" until a required reviewer approves (this is the human gate; nothing below runs first)
1. Checks out the repo with full history and tags (`fetch-depth: 0`) at the tagged commit
2. Installs the .NET SDKs needed to build all target frameworks
3. `dotnet restore` → `dotnet build -c Release` → `dotnet pack -c Release` — [MinVer](https://github.com/adamralph/minver) derives the package version from the `v*` tag at build time (`v1.4.6` → `1.4.6`); no csproj is patched. Commits without a tag get a unique prerelease version (e.g. `1.4.7-alpha.0.3`), so only tagged builds publish a stable release.
4. `dotnet nuget push *.nupkg --api-key $NUGET_API_KEY --skip-duplicate`

`--skip-duplicate` means re-running the workflow with the same version is a no-op rather than an error.

## After pushing the tag

- Watch the run: <https://github.com/maximn/google-maps/actions>
- **Approve the deployment** — the run pauses on "Review deployments"; approve the `release`
  environment to let the publish proceed (a reviewer can also reject it to abort)
- Once green, the new version appears at <https://www.nuget.org/packages/GoogleMapsApi/> (usually within a few minutes)

## Recovery

### Wrong tag pushed, workflow has NOT run yet (or has not published)

Delete the tag locally and remotely, then re-run:

```bash
git tag -d v1.4.6
git push origin :refs/tags/v1.4.6
```

### Wrong version already published to NuGet

NuGet does not allow re-uploading or hard-deleting a published version. Bump to the next patch and publish again:

```bash
./release.sh patch
```

You can optionally [unlist](https://learn.microsoft.com/en-us/nuget/nuget-org/policies/deleting-packages) the bad version on NuGet.org so it no longer appears in search results, but existing consumers who pinned to it can still restore it.

### Workflow failed mid-publish

Open the failed run in the Actions tab, fix the underlying issue, and either:
- Re-run the failed job (safe — `--skip-duplicate` handles the case where the package was already pushed), or
- Push a fresh tag with the next version
