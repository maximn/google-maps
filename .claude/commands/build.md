# Build Command

Builds the Google Maps API library project.

```bash
dotnet build
```

## Options

- `--no-restore`: Skip package restore
- `--configuration Release`: Build in release mode
- `--verbosity normal`: Set build verbosity level

## Usage Examples

```bash
# Standard build
dotnet build

# Build without restoring packages
dotnet build --no-restore

# Release build
dotnet build --configuration Release
```