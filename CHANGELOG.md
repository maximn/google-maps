# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog 1.1.0](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Migration from 1.5.x to 2.0

**Dropped target frameworks.** `net6.0`, `net462`, and `net481` are no longer explicit
targets. A GitHub code search across 45 public consumer csprojs (43 unique repos)
found **0 on net462/net481** and **0 on net6.0** — keeping them was paying CI minutes
and dependency-graph cost with no observable benefit. Framework consumers still
resolve via the `netstandard2.0` build; net6.0 reached EOL in November 2024 and
its consumers also resolve via `netstandard2.0` or upgrade to `net8.0`. No source
changes are required to consume the trimmed package.

**Static `GoogleMaps` facade is now `[Obsolete]` (warn-only, not error).** Existing
code keeps compiling and running. To migrate, switch to the instance-based client
shipped in 1.5.0:

```csharp
// Before
var response = await GoogleMaps.Geocode.QueryAsync(
    new GeocodingRequest { Address = "..." });

// After (works on 1.5.0+)
using var http = new HttpClient();
var maps = new GoogleMapsClient(http,
    new GoogleMapsClientOptions { ApiKey = "..." });
var response = await maps.Geocode.QueryAsync(
    new GeocodingRequest { Address = "..." });
```

The DI-friendly `services.AddGoogleMaps(...)` extension ships in 2.1.0. The
static facade is scheduled for removal in 3.0.

## [1.5.0] - 2026-05-25

### Added
- Instance-based `GoogleMapsClient` for `IHttpClientFactory` compatibility (#236)

### Changed
- Bump `testglance/action` to digest `29ac92e` (#241)
- Bump `actions/attest-build-provenance` to v4 (#238)
- Bump `actions/upload-pages-artifact` to v5 (#239)

### Fixed
- Handle pre-existing GitHub Release in NuGet publish workflow (#240)

## [1.4.8] - 2026-05-24

### Added
- SLSA provenance attestations and GitHub Release publishing for NuGet packages (#235)
- Sample apps: console, minimal API, and Blazor (#231)
- DocFX documentation site published via GitHub Pages (#226)
- XML documentation generation with comments on core public APIs (#232)
- OpenSSF Scorecard and CodeQL workflows with status badges (#228)
- CONTRIBUTING guide, Code of Conduct, and GitHub issue/PR templates (#222)

### Changed
- Promoted ReleaseNotes to root CHANGELOG.md in Keep a Changelog format (#227)
- Modernized README header, badges, and elevator pitch (#221)
- Repo hygiene: removed scratch files, expanded .gitignore, and dropped legacy docs dir (#223, #220)
- Bumped `actions/deploy-pages` to v5 (#230)
- Bumped `actions/configure-pages` to v6 (#229)

### Security
- Pinned all GitHub Actions to commit SHAs and added a Renovate digest preset (#237)
- Replaced `pull_request_target` with `pull_request` to resolve Scorecard Dangerous-Workflow finding (#234)
- Pinned `ossf/scorecard-action` to v2.4.3 (#233)

## [1.4.7] - 2026-05-23

### Added
- `net10.0` target framework (#216)

### Changed
- Generate release notes with `claude -p` and post them as a GitHub Release (#214)
- Add TestGlance for PR test summaries (#218)
- Drop `net6.0` from the test project (#215)
- Add `RELEASING.md` describing the release process (#212)
- Update .NET monorepo to v10 (#211)
- Update `Microsoft.NET.Test.Sdk` to v18 (#209)
- Update `coverlet.collector` to v10 (#208)
- Update `NUnit3TestAdapter` to v6 (#210)
- Update `actions/checkout` to v6 (#206)
- Update `actions/setup-dotnet` to v5 (#207)
- Update `actions/upload-artifact` to v7 (#213)

### Fixed
- Replace editor-based release-notes review with a terminal prompt (#219)

## [1.4.6] - 2026-05-23

### Added
- Configure Renovate for automated dependency updates (#194)

### Changed
- Tighten workflow permissions, drop EOL .NET 7, refresh `SECURITY.md` (#201)
- Check out the PR head SHA and gate `pull_request_target` to same-repo PRs (#200)
- Update NUnit to 4.6.1 (#204)
- Update `nunit.analyzers` to 4.13.0 (#205)
- Update `coverlet.collector` to 6.0.4 (#196)
- Update `Newtonsoft.Json` to 13.0.4 (#197)
- Update .NET monorepo dependencies (#202)

### Fixed
- Stabilize integration tests against Google API drift (#199)

## [1.4.5] - 2025-09-23

### Changed
- Bump `System.Text.Encodings.Web` from 5.0.0 to 5.0.1 (#193)

## [1.4.4] - 2025-09-19

### Changed
- Update `System.Text.Json` dependency to reference minimum supported versions per target framework (#188)

## [1.4.3] - 2025-09-19

_Re-release of 1.4.2; no functional changes._

## [1.4.2] - 2025-09-19

_Re-release of 1.4.1; no functional changes._

## [1.4.1] - 2025-09-19

### Changed
- Split CI workflow into separate build and test jobs

### Fixed
- Enable secrets access for external contributor PRs (#189)

## [1.4.0] - 2025-09-05

### Added
- Comprehensive test suite and JSON converter enhancements (#187)
- Docs directory

### Changed
- Complete Nullable Reference Types Migration — 644/644 warnings eliminated (#186)
- Fully migrate from Newtonsoft.Json to System.Text.Json (#180)
- Modernize HTTP Client Usage (#182)
- Un-ignore all tests (#181)
- Modernize readme files

### Fixed
- Static maps typos (#183)

## [1.3.9] - 2025-09-03

### Changed
- Update target versions (#178)

### Fixed
- Issue #141: Add Google Styling Wizard JSON support to StaticMapRequest (#177)

## [1.3.8] - 2025-09-03

### Fixed
- Issue #141: Add Google Styling Wizard JSON support to StaticMapRequest

## [1.3.7] - 2025-09-03

### Fixed
- Issue #147: Add helper methods to extract address components from Places Details API (#176)

## [1.3.6] - 2025-09-03

### Added
- Missing Google Places API properties to fix issue #157 (#175)
- Pagination support to Places Text Search API (#173)

### Changed
- Enhance release script to provide direct workflow links (#172)
- Revert "Enhance release script to provide direct workflow links (#172)" (#174)

## [1.3.5] - 2025-09-03

### Added
- Pagination support to Places Text Search API

## [1.3.4] - 2025-09-03

### Added
- Release automation script (#171)
- Development configuration and documentation (#168)

### Changed
- Update .NET version to 8.0.x in GitHub workflow (#169)

### Fixed
- Number formatting bug in `Location.ToString()` (#160) (#170)

## [1.3.3] - 2024-11-04

### Changed
- Updated dependencies and now compatible from netstandard2.0 to net8.0 (#166)

## [1.3.2] - 2024-10-15

### Added
- More .NET versions for NuGet (6/7/8)

## [1.3.1] - 2024-10-09

### Added
- Elevation API — added Resolution (#165)

### Changed
- Make the GitHub Actions to run on any PR

## [1.3.0] - 2024-10-09

### Changed
- Performance improvement — reuse single HttpClient

## [1.2.8] - 2024-10-09

### Changed
- Disable nullables (these fields are required but we never instantiate them directly, only via deserialization)

### Fixed
- `ShouldPassRawDataToOnRawResponseReceived`
- Warning — Exception shouldn't be serializable
- Typo (`OnRawResponseReceived`)

## [1.2.7] - 2024-10-09

### Added
- net7.0 target framework

### Changed
- Upgrade `actions/checkout` to v4

## [1.2.6] - 2024-10-09

### Added
- netstandard 2.0 support (#154)
- License.md file to the package

### Changed
- Publish for version tags
- Make sure it runs without the appsettings.json (on CI)
- Modernize tests — NUnit 4
- Modernize .csproj file
- Modernize dotnet.yml
- Modernize nuget.yml
- Make the build Release
- Use `PackageLicenseExpression` instead of the license file
- Skip duplicates on NuGet
- Upgrade dependency Newtonsoft.Json → 13.0.3 (#161)
- Bump Newtonsoft.Json from 13.0.1 to 13.0.2 (#156)
- Newtonsoft dep in .csproj
- Create SECURITY.md
- Markdown lint

### Removed
- Unused event `OnRawResponseRecivied` (later reverted)

### Fixed
- Tests warnings

## [1.2.0] - 2021-12-22

### Changed
- Improve `GeocodingRequest` (#145)
- Update dotnet.yml
- Update nuget.yml

## [1.0.1] - 2021-12-12

### Added
- Multiple .NET versions

### Changed
- Update `GoogleMapsApi.csproj`
- NuGet — `licenseUrl` → `license`
- Update nuget.yml

## [0.81.0] - 2021-12-12

### Added
- Find Place API call (#131)
- Support for `sessiontoken` parameter in `PlacesAutocompleteRequest` and `PlacesDetailsRequest` (#126)
- Missing `scale` parameter for `StaticMapRequest` (#122)
- Additional vehicle type (#119)
- Option for channel parameter in `SignableRequest` (#114)
- `PlaceId` to `GeocodingRequest` (#110)
- Strict Bounds to Places Autocomplete (#108)
- `ErrorMessage` to `DistanceMatrixResponse.cs` and several error codes to `DistanceMatrixStatusCodes` (#104)

### Changed
- Upgraded to .NET Core style project. Now supports .net 5, .net 6, .net standard 2.0, .NET Framework 4.6.1 (#144)
- Target only .net 6.0
- Change nuspec to target .NET 6 (from 4)
- GitHub Actions to .NET 6
- Change build status: Travis → GitHub Actions
- Set up GitHub Actions for .NET
- Handle async calls correctly with `ConfigureAwait(false)` (#120)
- Use `ConfigureAwait(false)` for blocking code
- Unit tests now show the actual error messages if status code wasn't 'OK' (#116)
- Hotfix/ferry improvements (#117)
- Add the configuration to workflows script
- Add the `SECRET_KEY` to GitHub workflows
- Try env specifically on tests
- AppxAutoIncrementPackageRevision
- Try to change the order of config loader
- Formatting
- `AppxAutoIncrementPackageRevision`

### Deprecated
- `sensor` parameter (#103)

### Removed
- `$version#` from csproj
- `.travis.yml`
- Duplicate env var key

### Fixed
- Fixed IT tests
- Fixed missing enum-value (#137)
- Fixed unit tests (#132)
- Fixed UT without ApiKey where needed (#140)
- Fix tests (#128)
- Add Newtonsoft dependency to nuspec (#127)

## [0.42] - 2015-10-17

### Added
- `IEngineFacade` to allow mocking and verification in consumer tests (#42)
- Radar search (#38)
- `.nuspec`
- `.travis.yml`
- Release notes file

### Changed
- Suppress warn 1519 in build (later reverted)
- Travis-ci badge

### Removed
- Files of Autocomplete desktop sample app

### Fixed
- Integration test due to new points in test of directions
- Test due to google `Avenue` → `Ave` change

## [0.41] (undated)

### Added
- `PlaceId` field (the `Id` and `References` fields are being deprecated in favor of `PlaceId`)
- Autocomplete APIs

### Fixed
- Sub steps for directions

## [0.30.0.0] - 2013-12-22

### Added
- `Geometry` property to Places Response
- `Geometry` property to Responses Result
- `Places\Response\Geometry` source
- Places Details query integration tests
- Opening hours to places details query
- Integration test for opening hours
- Price level, UTC offset and website values
- Location lat/long support to PlacesText API results
- `partial_match` support as requested in issue #19

### Changed
- Integration tests fixed with actual Google servers, version changed to 0.30.0.0
- Moved Places Details response status enum comments to XML doc format for intellisense
- Changed `PriceLevel` to strongly typed enum
- Arranged members alphabetically
- Changed code comments to XML comments for API consumers
- Corrected parameter name for the "location" query string
- Fixed the Geometry property XML comments

### Fixed
- Issue 2
- Added missing Places Details result status code enum values (unit test reproduced failed status code deserialization)

## [0.21.0.0] - 2013-04-06

### Added
- `PlacesDetails.Response.Result` Geometry property
- Synchronous query implementation
- Timeout support to synchronous API; updated `GoogleMaps` class to use synchronous APIs
- More integration tests since sync and async code paths have diverged
- Support for cancellation
- Support for specifying a timeout
- Integration tests for async operations, timeout and cancellation
- XML docs generation at build time to the 'Release' configuration
- `Microsoft.CompilerServices.AsyncTargetingPack` dependency providing Microsoft's official TaskEx class implementation for .NET 4.0
- Polylines for each step (issue #7)
- Lazy point decoding in `Route`
- Public method exposing the RAW encoded points
- Link to documentation of encoding
- Integration test for `OverviewPath`
- Internal `GoogleMaps` class with the suggested API
- `IResponseFor` marker interface for compile-time request/response type matching
- URL signing — new abstract `SignableRequest` class; if credentials are provided, `GetUri` produces a signed URI
- `AuthorizationException` thrown when a request fails due to incorrect credentials
- Per-engine control of HTTP and HTTPS connection limits for high-throughput scenarios
- `*.user` to `.gitignore`

### Changed
- Visual Studio 2012 / FW 4.5
- `OptimizeWaypoints` (issue #6)
- `ILocation` → `ILocationString`
- Changed internal serialization implementation protection level from public to internal
- `QueryStringParametersList` changed
- More generic approach for creating query string in StaticMaps
- Made the default timeout of 100 seconds a static readonly field
- Split a complex line of code for clarity
- Step.TravelMode as auto property
- Moved time conversion to Unix time to class — `UnixTimeConverter`
- Made `GoogleMaps` public and added XML docs; constructor made private
- Made the methods of `MapsAPIGenericEngine` static
- Switched `StaticMapsEngine` to use `QueryStringParametersList`
- Made StaticMaps tests use the AAA pattern (Arrange-Act-Assert)
- Moved the query string creation from the WebClient `.QueryString` to a virtual method on `MapsBaseRequest` that returns a `QueryStringParametersList`
- Moved `BaseUrl` to `MapsBaseRequest` class
- Moved Uri creation to the virtual `GetUri()` method of `MapsBaseRequest` class
- Changed WebClient async usage to `DownloadDataTaskAsync` instead of `DownloadStringTaskAsync` so that an intermediary string isn't created
- Spaces → tabs (reformat code)
- File version advanced to 0.12.0.0
- Version 0.11.0.0

### Deprecated
- Marked the existing APIs with `[Obsolete]`

### Removed
- Dependency on `Microsoft.CompilerServices.AsyncTargetingPack`
- Unofficial `TaskEx` class
- Use of completion handler from synchronous version
- `DownloadDataTaskAsync(WebClient, Uri)` overload (already provided by AsyncTargetingPack)
- Duplicate unused files probably mistakenly pushed during the move from Google Code to GitHub
- Subversion reference from solution file

### Fixed
- Bug where cancelling a request before the download has started was ignored
- `DownloadDataTaskAsync()` now unregisters the event handler when done (prevents memory leaks if WebClient is reused outside this library)
- Added parameter validation to `WebClientEx` constructor
- Fixed malformed XML comments
- Fixed XML docs
- Fixed issue #4
- "jpg-baselin" → "jpg-baseline" in `StaticMapsEngine`

[Unreleased]: https://github.com/maximn/google-maps/compare/v1.5.0...HEAD
[1.5.0]: https://github.com/maximn/google-maps/compare/v1.4.8...v1.5.0
[1.4.8]: https://github.com/maximn/google-maps/compare/v1.4.7...v1.4.8
[1.4.7]: https://github.com/maximn/google-maps/compare/v1.4.6...v1.4.7
[1.4.6]: https://github.com/maximn/google-maps/compare/v1.4.5...v1.4.6
[1.4.5]: https://github.com/maximn/google-maps/compare/v1.4.4...v1.4.5
[1.4.4]: https://github.com/maximn/google-maps/compare/v1.4.3...v1.4.4
[1.4.3]: https://github.com/maximn/google-maps/compare/v1.4.2...v1.4.3
[1.4.2]: https://github.com/maximn/google-maps/compare/v1.4.1...v1.4.2
[1.4.1]: https://github.com/maximn/google-maps/compare/v1.4.0...v1.4.1
[1.4.0]: https://github.com/maximn/google-maps/compare/v1.3.9...v1.4.0
[1.3.9]: https://github.com/maximn/google-maps/compare/v1.3.8...v1.3.9
[1.3.8]: https://github.com/maximn/google-maps/compare/v1.3.7...v1.3.8
[1.3.7]: https://github.com/maximn/google-maps/compare/v1.3.6...v1.3.7
[1.3.6]: https://github.com/maximn/google-maps/compare/v1.3.5...v1.3.6
[1.3.5]: https://github.com/maximn/google-maps/compare/v1.3.4...v1.3.5
[1.3.4]: https://github.com/maximn/google-maps/compare/v1.3.3...v1.3.4
[1.3.3]: https://github.com/maximn/google-maps/compare/v1.3.2...v1.3.3
[1.3.2]: https://github.com/maximn/google-maps/compare/v1.3.1...v1.3.2
[1.3.1]: https://github.com/maximn/google-maps/compare/v1.3.0...v1.3.1
[1.3.0]: https://github.com/maximn/google-maps/compare/v1.2.8...v1.3.0
[1.2.8]: https://github.com/maximn/google-maps/compare/v1.2.7...v1.2.8
[1.2.7]: https://github.com/maximn/google-maps/compare/v1.2.6...v1.2.7
[1.2.6]: https://github.com/maximn/google-maps/compare/v1.2.0...v1.2.6
[1.2.0]: https://github.com/maximn/google-maps/compare/v1.0.1...v1.2.0
[1.0.1]: https://github.com/maximn/google-maps/compare/v0.81.0...v1.0.1
[0.81.0]: https://github.com/maximn/google-maps/compare/0.42...v0.81.0
[0.42]: https://github.com/maximn/google-maps/compare/0.30.0.0...0.42
[0.41]: https://github.com/maximn/google-maps/tree/v0.41
[0.30.0.0]: https://github.com/maximn/google-maps/compare/0.21.0.0...0.30.0.0
[0.21.0.0]: https://github.com/maximn/google-maps/releases/tag/0.21.0.0
