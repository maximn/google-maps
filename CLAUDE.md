# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Google Maps API .NET Library

This is a comprehensive .NET wrapper library for Google Maps Web Services APIs, providing strongly-typed requests and responses for geocoding, directions, elevation, places, and other Google Maps services.

## Project Architecture

### Core Components
- **GoogleMaps.cs**: Static entry point providing access to all API services via facade pattern
- **EngineFacade<TRequest, TResponse>**: Generic facade implementing async/sync query operations with timeout and cancellation support
- **MapsAPIGenericEngine<TRequest, TResponse>**: Core HTTP client engine using Newtonsoft.Json for serialization
- **Static Maps**: Separate engine for generating Google Static Maps URLs with marker, path, and styling support

### Request/Response Pattern
All API operations follow a consistent pattern:
- Request entities inherit from `MapsBaseRequest` with `GetUri()` method for URL generation
- Response entities implement `IResponseFor<TRequest>` interface
- Generic type constraints ensure compile-time request/response matching

### Supported APIs
- **Geocoding**: Address ↔ coordinates conversion
- **Directions**: Route planning with multiple travel modes
- **Elevation**: Elevation data for locations/paths
- **Places**: Search (nearby, text, find), details, autocomplete
- **Distance Matrix**: Travel time/distance between multiple origins/destinations
- **Time Zone**: Time zone data for coordinates
- **Static Maps**: URL generation for static map images

## Development Commands

### Essential Commands
```bash
# Restore dependencies
dotnet restore

# Build (targets: net8.0, net7.0, net6.0, netstandard2.0)
dotnet build

# Run tests with API key
dotnet test --verbosity normal
# Alternative: GOOGLE_API_KEY=your_key dotnet test

# Format code
dotnet format

# Create NuGet package
dotnet pack --no-build
```

### Test Configuration
Integration tests require Google API key via:
1. Environment variable: `GOOGLE_API_KEY=your_api_key`
2. Test settings file: `GoogleMapsApi.Test/appsettings.json` (copy from `appsettings.template.json`)

### Single Test Execution
```bash
# Run specific test class
dotnet test --filter "ClassName=DirectionsTests"

# Run specific test method
dotnet test --filter "TestMethodName=Should_Get_Directions"
```

## Code Architecture Guidelines

### Adding New API Support
1. Create request/response entities in `Entities/{ServiceName}/`
2. Request class inherits `MapsBaseRequest`, implements `GetUri()`
3. Response class implements `IResponseFor<RequestType>`
4. Add service property to `GoogleMaps.cs` following existing pattern
5. Add integration tests in `GoogleMapsApi.Test/IntegrationTests/`

### Multi-Framework Compatibility
- Main library targets: net8.0, net7.0, net6.0, netstandard2.0
- Tests target: net8.0, net6.0, net4.8
- Use conditional compilation when framework-specific features needed

### HTTP Client Usage
- Single static HttpClient instance in `MapsAPIGenericEngine`
- Async-only operations with timeout and cancellation token support
- Custom extension method `DownloadDataTaskAsyncAsString` for HTTP operations

### Event System
- `OnUriCreated`: Allows URL modification before requests
- `OnRawResponseReceived`: Access to raw JSON responses for debugging/logging

## Testing Strategy

### Integration Tests
- Test against live Google APIs (count towards quota)
- Require internet connection and valid API key
- Located in `GoogleMapsApi.Test/IntegrationTests/`
- Inherit from `BaseTestIntegration` for API key management

### Test Naming
Use descriptive test names explaining expected behavior:
```csharp
[Test]
public void Should_Get_Valid_Directions_Between_Two_Addresses()
```

## Package Configuration

### NuGet Package
- Multi-framework targeting for broad compatibility
- Automatic version increment in CI/CD
- Source linking enabled for debugging
- Symbols package (.snupkg) generation
- GitHub Actions automated publishing to NuGet.org
- 
