# Format Command

Formats C# code using .NET's built-in formatter.

```bash
dotnet format
```

## Code Style

The project targets multiple .NET versions and follows standard C# conventions:
- **Target Frameworks**: net8.0, net7.0, net6.0, netstandard2.0
- **Language Version**: Latest C#
- **Nullable**: Disabled in main project, enabled in tests

## Options

- `--verify-no-changes`: Verify no formatting changes needed
- `--include`: Include specific files/folders
- `--exclude`: Exclude specific files/folders

## Usage Examples

```bash
# Format all files
dotnet format

# Verify formatting
dotnet format --verify-no-changes

# Format specific project
dotnet format GoogleMapsApi/GoogleMapsApi.csproj
```