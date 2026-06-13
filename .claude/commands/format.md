# Format Command

Formats C# code using .NET's built-in formatter.

```bash
dotnet format
```

## Code Style

The project targets multiple .NET versions and follows standard C# conventions:
- **Target Frameworks**: netstandard2.0, net8.0, net10.0
- **Language Version**: Latest C#
- **Nullable**: Enabled

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