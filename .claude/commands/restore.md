# Restore Command

Restores NuGet package dependencies for the Google Maps API project.

```bash
dotnet restore
```

## Dependencies

The project uses these main dependencies:
- **Newtonsoft.Json** (13.0.3) - JSON serialization
- **Microsoft.SourceLink.GitHub** (8.0.0) - Source linking

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