# Test Command

Runs the NUnit test suite for the Google Maps API library. Test projects target `net8.0` and `net10.0`.

```bash
dotnet test --verbosity normal
```

## Environment Requirements

Integration tests hit the **live** Google APIs and require a `GOOGLE_API_KEY` (environment variable, or
`GoogleMapsApi.Test/appsettings.json` copied from `appsettings.template.json`). Unit tests are hermetic
and need no key.

## Billable (Places) tests — skipped by default

Tests tagged `[BillableTest]` (NUnit category `Billable`) are **skipped** unless `RUN_BILLABLE_TESTS`
is truthy (`1`/`true`/`yes`). See `.agents/testing.md`.

## Options

- `--no-build`: Run tests without building
- `--configuration Release`: Run tests in release mode
- `--framework net8.0|net10.0`: Run a single target framework
- `--collect:"XPlat Code Coverage"`: Collect code coverage

## Usage Examples

```bash
# Run all tests (all TFMs, billable tests skipped)
dotnet test --verbosity normal

# Run a single target framework
dotnet test --framework net10.0

# Run a single fixture
dotnet test --filter "ClassName=DirectionsTests"

# Include billable Places tests (incurs charges)
RUN_BILLABLE_TESTS=true dotnet test

# Run ONLY the billable tests
RUN_BILLABLE_TESTS=true dotnet test --filter "TestCategory=Billable"

# Code coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Test Configuration

Configured in `GoogleMapsApi.Test.csproj`:
- NUnit + NUnit3TestAdapter
- Microsoft.NET.Test.Sdk
- coverlet for code coverage
