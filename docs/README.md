# Documentation site

This directory contains the [DocFX](https://dotnet.github.io/docfx/) project that builds the public documentation site for **GoogleMapsApi**, published to <https://maximn.github.io/google-maps>.

## What's here

- `docfx.json` — DocFX configuration. API metadata is extracted from `../GoogleMapsApi/GoogleMapsApi.csproj` (TFM: `net8.0`).
- `index.md`, `toc.yml` — site landing page and root navigation.
- `articles/` — hand-written guides (Getting Started, etc.).
- `api/` — *generated*, do not edit by hand.
- `_site/` — *generated* output, do not commit.

## Regenerate locally

You need the .NET SDK (8.0 or newer) and the `docfx` global tool:

```bash
dotnet tool install -g docfx
# or, if already installed:
dotnet tool update -g docfx
```

From the repository root:

```bash
# Generate metadata + build static site
docfx docs/docfx.json

# Build and serve at http://localhost:8080
docfx docs/docfx.json --serve
```

The built site lands in `docs/_site/`.

## Deployment

Deployment is automated by `.github/workflows/docs.yml`:

- Triggers on push to `master` and via `workflow_dispatch`.
- Builds with the official `docfx` global tool.
- Publishes to GitHub Pages via `actions/deploy-pages`.

> **One-time setup (maintainer):** in the repository settings, navigate to **Settings → Pages** and set **Source** to **GitHub Actions** before the first deploy.
