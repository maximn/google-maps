# Public API lock

The public surface is frozen and **CI-enforced**, so accidental breaking changes can't slip in. If you
add, change, or remove anything `public`, you must update the API files or **the build fails**.

## How it works

- `GoogleMapsApi.csproj` references `Microsoft.CodeAnalysis.PublicApiAnalyzers` and includes two
  tracking files as `AdditionalFiles`:
  - `GoogleMapsApi/PublicAPI.Shipped.txt` — the baseline already released (large; treat as append-only
    history).
  - `GoogleMapsApi/PublicAPI.Unshipped.txt` — public members added since the last release.
- `GoogleMapsApi/.editorconfig` sets the analyzer rules to **`error`** severity: `RS0016`, `RS0017`,
  `RS0024`, `RS0025`, `RS0026`, `RS0027`, `RS0036`, `RS0037`, `RS0041`, `RS0048`, `RS0050`.

What the common ones mean:
- **RS0016** — a public symbol is missing from the API files (you added public API but didn't record it).
- **RS0017** — the API file lists a symbol that no longer exists (you removed/renamed public API).
- **RS0026** — entries must be **sorted**.
- **RS0036 / RS0037 / RS0041** — nullable annotations must be tracked; the file needs the
  `#nullable enable` header.

## When you change the public surface

1. Build — the analyzer tells you exactly which entries are missing/extra.
2. Add new members to **`PublicAPI.Unshipped.txt`** in the analyzer's exact format (full signature),
   keeping the file **sorted** and the `#nullable enable` header intact.
3. For a removal/rename, delete (or move) the matching line from the API files.
4. Easiest path: let the IDE/analyzer **code fix** ("Add to public API") do the formatting, or run
   `dotnet format analyzers`, then review the diff.
5. At release time, `release.sh` / the release flow moves `Unshipped` entries into `Shipped`. In a
   normal feature PR you only touch `Unshipped.txt`.

## Notes

- The files are large and format-sensitive (sorting, nullable markers). Prefer the analyzer's auto-fix
  over hand-editing to avoid `RS0026`/`RS0041` churn.
- This is the mechanism that makes 2.0's breaking changes (removed static facade, removed legacy Places)
  explicit rather than silent. Respect it — don't relax the severities to make a build pass.
- Project memory: the surface is intentionally locked; public changes must update these files.
