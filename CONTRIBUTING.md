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

The library targets net8.0, net6.0, netstandard2.0, net481, and net462. You need a recent
.NET SDK installed.

```bash
dotnet restore
dotnet build
dotnet test
```

The integration tests hit the live Google Maps APIs and require an API key. Provide it
via the `GOOGLE_API_KEY` environment variable, or copy
`GoogleMapsApi.Test/appsettings.template.json` to `appsettings.json` and fill in the value.
Note that running the test suite consumes your Google API quota.

### Billable tests (Places API)

Some Google APIs — currently **Places** — exceed the free quota and incur charges. Their
integration fixtures are tagged `[BillableTest]` (category `Billable`, see
`GoogleMapsApi.Test/Utils/BillableTestAttribute.cs`) and are **skipped by default** so a plain
`dotnet test` never runs up the bill. Everything else runs as usual.

Opt in only when you specifically need them, by setting the `RUN_BILLABLE_TESTS` environment
variable to a truthy value (`1`, `true`, or `yes`):

```bash
# Run the full suite, including billable tests
RUN_BILLABLE_TESTS=true dotnet test

# Run only the billable tests
RUN_BILLABLE_TESTS=true dotnet test --filter "TestCategory=Billable"
```

When adding a fixture that calls a billable API, annotate it with `[BillableTest]` so it stays
off by default.

#### Running billable tests in CI

The same opt-in is wired into the `.NET` GitHub Actions workflow (`.github/workflows/dotnet.yml`),
which keeps billable tests skipped on ordinary push/PR runs. Two ways to run them on demand:

- **PR label** — add the `run-billable-tests` label to a pull request. Applying the label starts a
  run, and it stays in effect while the label is present. (Labels can only be applied by
  collaborators, and the workflow only runs for same-repo branches, so external PRs can't trigger
  paid tests.)
- **Manual run** — from the **Actions** tab, open the **.NET** workflow, click **Run workflow**, and
  check **Run billable API tests (Places)**.

## Style

Run `dotnet format` before pushing. It applies the repo's .editorconfig rules and keeps
diffs focused on real changes.

Prefer self-documenting code over comments. Comments should explain *why* something
non-obvious is being done, not *what* the code is doing.

## Pull request checklist

Before opening a PR, please confirm:

- [ ] `dotnet format` has been run.
- [ ] `dotnet build` succeeds across all target frameworks.
- [ ] `dotnet test` passes locally (with a valid `GOOGLE_API_KEY`).
- [ ] New public API surface has XML documentation comments.
- [ ] If you fixed a bug, a regression test covers it.
- [ ] The PR description links the related issue and explains the change.

Smaller, focused PRs land faster than large ones. If you are planning a substantial
change, open an issue or discussion first so we can agree on the approach.

## Translations welcome

If you would like to translate the README into another language, please open a PR adding
`README.<lang>.md` at the repo root and link it from the main README. Translations are a
great first contribution.
