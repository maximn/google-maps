# Restore Command

Restores NuGet package dependencies for the Google Maps API project.

```bash
dotnet restore
```

## Dependencies

The main library uses these dependencies:
- **System.Text.Json** - JSON serialization (on `netstandard2.0`/`net8.0`; in-box on `net10.0`)
- **Microsoft.SourceLink.GitHub** - Source linking (build-only)
- **MinVer** - Version derivation from git tags (build-only)
- **Microsoft.CodeAnalysis.PublicApiAnalyzers** - Public-API surface lock (build-only)

## Test Dependencies

- **Microsoft.NET.Test.Sdk** - Test SDK
- **NUnit** - Testing framework
- **NUnit3TestAdapter** - Test adapter
- **coverlet.collector** - Code coverage

## Usage Examples

```bash
# Restore all packages
dotnet restore

# Restore for specific project
dotnet restore GoogleMapsApi/GoogleMapsApi.csproj
```