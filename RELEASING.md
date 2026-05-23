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
- The `NUGET_API_KEY` secret is configured in repo settings (one-time setup; already in place)

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
4. Prompts `y/N` before doing anything
5. On confirmation, creates the tag locally and pushes it to `origin`

## Option 2: Tag manually

If you want to pick an explicit version (out-of-band release, hotfix on an older line, etc.):

```bash
git tag v1.4.6
git push origin v1.4.6
```

Both options end at the same place — a `v*` tag in `origin` — and trigger the same workflow.

## Choosing the bump type (semver)

- **patch** — bug fixes, dependency bumps, internal cleanup; no public API change
- **minor** — new functionality, backwards-compatible additions
- **major** — breaking changes to the public API

## What the workflow does

[`.github/workflows/nuget.yml`](.github/workflows/nuget.yml) runs on `push` of any `v*` tag:

1. Checks out the repo at the tagged commit
2. Installs the .NET SDKs needed to build all target frameworks
3. Extracts the version from the tag (`v1.4.6` → `1.4.6`)
4. Injects the version into `GoogleMapsApi/GoogleMapsApi.csproj` (the csproj's `<Version>0.0.0</Version>` placeholder stays unchanged in the repo — only the runner copy is patched)
5. `dotnet restore` → `dotnet build -c Release` → `dotnet pack -c Release`
6. `dotnet nuget push *.nupkg --api-key $NUGET_API_KEY --skip-duplicate`

`--skip-duplicate` means re-running the workflow with the same version is a no-op rather than an error.

## After pushing the tag

- Watch the run: <https://github.com/maximn/google-maps/actions>
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
