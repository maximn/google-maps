# Test Command

Runs NUnit tests for the Google Maps API library.

```bash
dotnet test --verbosity normal
```

## Environment Requirements

Tests require a `GOOGLE_API_KEY` environment variable for integration tests.

## Options

- `--no-build`: Run tests without building
- `--configuration Release`: Run tests in release mode
- `--verbosity normal`: Set test output verbosity
- `--collect:"XPlat Code Coverage"`: Collect code coverage

## Usage Examples

```bash
# Run all tests
dotnet test --verbosity normal

# Run tests without building
dotnet test --no-build --verbosity normal

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Test Configuration

Tests are configured in `GoogleMapsApi.Test.csproj` using:
- NUnit framework
- Microsoft.NET.Test.Sdk
- Coverlet for code coverage