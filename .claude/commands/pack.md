# Pack Command

Creates NuGet packages for the Google Maps API library.

```bash
dotnet pack --no-build
```

## Package Configuration

Configured in `GoogleMapsApi.csproj`:
- **Multi-targeting**: netstandard2.0, net8.0, net10.0
- **Auto-increment**: Package revision auto-increments
- **Generate on build**: Packages created during build
- **Symbols**: Debug symbols included (.snupkg)

## Options

- `--no-build`: Pack without building
- `--configuration Release`: Pack release build
- `--output`: Specify output directory

## Usage Examples

```bash
# Pack current build
dotnet pack --no-build

# Pack release build
dotnet pack --configuration Release --no-build

# Pack with custom output
dotnet pack --output ./packages
```

## GitHub Actions

The project uses GitHub Actions for automated NuGet publishing on releases.