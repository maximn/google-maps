# Contributing to GoogleMapsApi

Thanks for your interest in improving the library. This project is a community-maintained
.NET wrapper for Google Maps Web Services APIs, run by a single maintainer with help from
contributors like you. Issues and pull requests are reviewed as time allows, so please be
patient and constructive.

## Filing a bug

If something does not work the way you expect, please open a bug report using the
[bug issue template](.github/ISSUE_TEMPLATE/bug.yml). A minimal repro (a few lines of C#
that reproduce the failure) is the single most useful thing you can include.

## Proposing a feature

For new APIs, parameters, or behaviour changes, open a feature request using the
[feature issue template](.github/ISSUE_TEMPLATE/feature.yml). Describe the use case first
and the proposed API shape second. For open-ended questions or design discussions, use
[GitHub Discussions](https://github.com/maximn/google-maps/discussions) instead of an issue.

## Local development quick start

The library targets netstandard2.0, net8.0, and net10.0. You need a recent
.NET SDK installed.

```bash
dotnet restore
dotnet build
dotnet test
```

The integration tests default to **replay mode**: they serve responses from committed cassettes
under `GoogleMapsApi.Test/Cassettes/`, so a plain `dotnet test` runs **offline, with no API key, and
no charges**. You don't need a Google key to contribute or to run the suite. A test whose cassette
hasn't been recorded yet **fails loudly**, so missing integration coverage cannot pass unnoticed.

### Recorded-response (VCR) test modes

The mode is chosen by the `VCR_MODE` environment variable (default `replay`):

| `VCR_MODE` | What it does | Needs `GOOGLE_API_KEY`? | Charges? |
| --- | --- | --- | --- |
| `replay` *(default)* | Serve responses from committed cassettes; fail loudly if the cassette or request is missing | No | No |
| `record` | Call live Google and **replace** cassettes on disk, then commit them | Yes | Yes |
| `auto` | Replay on a cassette hit, record live on a miss | Yes (on misses) | On misses |
| `live` | Pass straight through to Google, never touching cassettes (drift check) | Yes | Yes |

To **(re)record** cassettes — needed when you add a test, or when Google's responses change — run
record mode with a real key, then commit the resulting JSON:

```bash
# Record everything (incl. billable APIs), then commit the cassettes
VCR_MODE=record GOOGLE_API_KEY=<key> RUN_BILLABLE_TESTS=true dotnet test

# Re-record a single fixture
VCR_MODE=record GOOGLE_API_KEY=<key> dotnet test --filter "ClassName=GeocodingTests"
```

Cassettes redact the `key`/`signature` query parameters, so no secret is ever committed.

### Billable tests (Places, Aerial View, Solar)

Some Google APIs exceed the free quota and incur charges. Their fixtures are tagged `[BillableTest]`
(category `Billable`, see `GoogleMapsApi.Test/Utils/BillableTestAttribute.cs`). The gate only applies
in a **live mode** (`record`/`auto`/`live`), where the test actually calls Google: there it's skipped
unless `RUN_BILLABLE_TESTS` is truthy (`1`, `true`, or `yes`). In the default `replay` mode the
response comes from a cassette — free and deterministic — so **billable tests run normally offline**.

```bash
# Replay everything offline, including billable fixtures (the default)
dotnet test

# Run billable fixtures LIVE (recording or drift-checking) — incurs charges
VCR_MODE=live RUN_BILLABLE_TESTS=true GOOGLE_API_KEY=<key> dotnet test --filter "TestCategory=Billable"
```

When adding a fixture that calls a billable API, annotate it with `[BillableTest]` so live runs stay
opt-in.

#### CI

The `.NET` GitHub Actions workflow (`.github/workflows/dotnet.yml`) always runs unit tests and the VCR
harness offline. It also runs the `Integration` category in replay once cassette JSON files have been
committed; until the initial cassette dataset is seeded, CI emits an explicit warning instead. A
separate **scheduled / manually-dispatched** job runs the integration tests **live**
(`VCR_MODE=live`, with `GOOGLE_API_KEY` + `RUN_BILLABLE_TESTS`) to catch Google API drift.

## Style

Run `dotnet format` before pushing. It applies the repo's .editorconfig rules and keeps
diffs focused on real changes.

Prefer self-documenting code over comments. Comments should explain *why* something
non-obvious is being done, not *what* the code is doing.

## Pull request checklist

Before opening a PR, please confirm:

- [ ] `dotnet format` has been run.
- [ ] `dotnet build` succeeds across all target frameworks.
- [ ] `dotnet test` passes locally (offline, in the default `replay` mode — no key needed).
- [ ] If you added or changed an integration test, you re-recorded and committed its cassette
      (`VCR_MODE=record … dotnet test`).
- [ ] New public API surface has XML documentation comments.
- [ ] If you fixed a bug, a regression test covers it.
- [ ] The PR description links the related issue and explains the change.

Smaller, focused PRs land faster than large ones. If you are planning a substantial
change, open an issue or discussion first so we can agree on the approach.

## Translations welcome

If you would like to translate the README into another language, please open a PR adding
`README.<lang>.md` at the repo root and link it from the main README. Translations are a
great first contribution.
