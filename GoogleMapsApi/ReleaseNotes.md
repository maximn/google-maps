# Release notes

## V 1.4.0
* Add comprehensive test suite and JSON converter enhancements (#187)
* Complete Nullable Reference Types Migration - 644/644 Warnings Eliminated (#186)
* Fully migrate from Newtonsoft.Json to System.Text.Json (#180)
* Modernize HTTP Client Usage (#182)
* Fix static maps typos (#183)
* Un-ignore all tests (#181)
* Modernize readme files
* Add docs directory

## V 1.3.9
* Update target versions (#178)
* Fix issue #141: Add Google Styling Wizard JSON support to StaticMapRequest (#177)

## V 1.3.8
* Fix issue #141: Add Google Styling Wizard JSON support to StaticMapRequest

## V 1.3.7
* Fix issue #147: Add helper methods to extract address components from Places Details API (#176)

## V 1.3.6
* Add missing Google Places API properties to fix issue #157 (#175)
* Revert "Enhance release script to provide direct workflow links (#172)" (#174)
* Add pagination support to Places Text Search API (#173)
* Enhance release script to provide direct workflow links (#172)

## V 1.3.5
* Add pagination support to Places Text Search API

## V 1.3.4
* Add release automation script (#171)
* Fix number formatting bug in Location.ToString() (#160) (#170)
* Add development configuration and documentation (#168)
* Update .NET version to 8.0.x in GitHub workflow (#169)

## V 1.3.3
* Updated dependencies and now compatible from netstandard2.0 to net8.0 (#166)

## V 1.3.2
* More .net versions for nuget (6/7/8)

## V 1.3.1
* Elevation API - Added Resolution (#165)
* Make the github actions to run on any PR

## V 1.3.0
* Performance improvement - Reuse single HttpClient

## V 1.2.8
* Fix 'ShouldPassRawDataToOnRawResponseReceived'
* Fix warning - Exception shouldn't be serializable
* Disable nullables (As these fields are required but we never instantiate them directly, but by deserialization)
* Fix typo (OnRawResponseReceived)

## V 1.2.7
* Adding net7.0 target framework
* Upgrade actions/checkout to v4

## V 1.2.6
* Publish for version tags
* Make sure it runs without the appsettings.json (on CI)
* Fix tests warnings
* Modernize tests - NUnit 4
* Modernize .csproj file
* Markdown lint
* Revert "Removed unused event OnRawResponseRecivied"
* Removed unused event OnRawResponseRecivied
* Use PackageLicenseExpression instead of the license file
* Added license.md file to the package
* Skip duplicates on nuget
* Modernize dotnet.yml
* Remove comments
* Modernize nuget.yml
* Version 1.2.3
* Debug another folder
* Make the build Release
* Update nuget.yml to debug the .nupkg file
* Newtonsoft dep in .csproj
* Version 1.2.2 (#162)
* Upgrade dependency Newtonsoft.Json -> 13.0.3 (#161)
* Create SECURITY.md
* Bump Newtonsoft.Json from 13.0.1 to 13.0.2 (#156)
* Feature: netstandard 2.0 support (#154)

## V 1.2.0
* Version 1.2.0
* Improve GeocodingRequest. (#145)
* Revert "Update dotnet.yml"
* Update dotnet.yml
* Update nuget.yml
* Version

## V 1.0.1
* Update nuget.yml
* Multiple .NET versions
* Update GoogleMapsApi.csproj
* Nuget - licenseUrl -> license

## V 0.81.0
* Update nuget.yml
* Removed duplicate env var key
* Revert "removed duplicate env var key"
* Removed duplicate env var key
* Try env specifically on tests
* Formatting
* Revert "Add the configuration to workflows script"
* Target only .net 6.0
* Add the configuration to workflows script
* Add the SECRET_KEY to github workflows
* Try to change the order of config loader
* Fixed IT tests
* AppxAutoIncrementPackageRevision
* Removed $version# from csproj
* Update GoogleMapsApi.sln
* Delete .travis.yml
* Change nuspec to target .NET 6 (from 4)
* Github actions to .NET 6
* Change build status: Travis -> Github actions
* Set up Github actions for .NET
* Upgraded to .Net Core style project. Now supports .net 5, .net 6, .net standard 2.0, net framework 4.6.1 (#144)
* Fixed UT without ApiKey where needed. (#140)
* Fixed missing enum-value. (#137)
* Fix unit tests (#132)
* Added Find Place API call (#131)
* Fix tests (#128)
* Add Newtonsoft dependency to nuspect (#127)
* Implement support for sessiontoken parameter in PlacesAutocompleteRequest and PlacesDetailsRequest (#126)
* Add missing 'scale' parameter for StaticMapRequest (#122)
* Handle async calls correctly with ConfigureAwait(false) (#120)
* Feature/additional vehicle type (#119)
* Unit tests now shows the actual errormessages if statuscode wasn't 'OK' (#116)
* Hotfix/ferry improvements (#117)
* Added option for channel parameter in SignableRequest (#114)
* Add PlaceId to GeocodingRequest (#110)
* Adding Strict Bounds to Places Autocomplete (#108)
* Dummy commit to trigger travisci
* Use ConfigureAwait(false) for blocking code
* Added ErrorMessage to DistanceMatrixResponse.cs and few error codes to DistanceMatrixStatusCodes (#104)
* Made sensor parameter obsolete (#103)
* Use TravisCI build badge
* Update Travis Nunit version (#101)
* Fix for failing unit test (#100)
* Net core web client removal (#99)
* Moving towards a netcore style project layout (#98)
* Unignoring test
* Fixed IT with google
* Fixed spaces. (#93)
* Fix mono version, latest have a bug with http client
* Use API Key for GeocodingAsync_ReturnsCorrectLocation
* Ignore test that fails on CI from unknown reason
* Revert "Checking if the connection limit increase will fix the build"
* Checking if the connection limit increase will fix the build
* Fixed IT with google
* Fix IT with google
* Fix integration test with google
* Unittest fix: test failed with an unhandled exception instead of proper Assertion Message when object under test is null (#83)
* Added support of API Key while creating static map request (#82)
* Types have been deprecated and have been replaced with Type. (#79)
* Adding support for directions with traffic (#78)
* Making the LocationString generating a period in all cultures (#76)
* Update StaticMaps.cs
* Fix zero-longitude location string (should be "0.0", not "") (#73)
* Update StaticMaps.cs
* Fix Incorrect LocationString Longitude Near Zero (#71)
* Fix GeocodingRequest get parameters. (#69)
* Improvement in GeocodingRequest adding Component Filtering (#67)
* Variables should be changed before the v1.0. (#68)
* Added optional parameters to DistanceMatrixRequest (#65)
* Make test more tolerant
* Added the region property so that directions results can be localized (#63)
* Fixed IT with google
* Add Photos and PermanentlyClosed (#62)
* Added Bounds to Route (#59)
* Update README.md
* Add local icon url to Vehicle model (#58)
* Fixed UNIX timestamp daylight savings time issue
* NUnit3 in travis
* Travis nunit console
* Removed un-needed test
* Update travis to nuget 3.2.0
* Merge pull request #51 from quasilatent/master
* Removing radius limitation for PlacesAutocomplete to enable users to turn off location bias
* Proj
* Upgraded nUnit to 3 and removed nuget async package, now it's available in the FW
* Changing test to the new direction request
* Example from google api docs
* Get rid of the DepartureTime which might cause integration test to fail
* Separate lines to have more clear error on failure
* Update DirectionsTests.cs
* Changed directions request (transit) DepartureTime
* Merge pull request #49 from CxSoftware/add_directionsresponse_errormessage
* Adding test for DirectionsResponse.ErrorMessage
* Adding DirectionsResponse.ErrorMessage documentation
* Adding property DirectionsResponse.ErrorMessage
* Merge pull request #50 from pirvudoru/gm-places-photos
* Add Photos to Places TextSearch
* Merge pull request #48 from MatteoDuranti/patch-1
* Update Result.cs
* Merge pull request #47 from solmead/master
* Added Integration Test, Added Response Type as well to better match existing pattern
* Added ability to do a near by places search.
* Update README.md
* Revert "Update PlacesTextTests.cs"
* Revert "removed directory which shouldn't be part of the code"
* Update PlacesTextTests.cs
* Update PlaceAutocompleteTests.cs
* Merge pull request #46 from vanko-dev/master
* Changed signature of UriCreatedDelegate delegate
* Two events for interception purposes have been added: OnUriCreated OnRawResponseRecivied
* Support of "Google Maps Distance Matrix API" has been added
* Update readme
* Removed directory which shouldn't be part of the code
* Fixed test syntax
* Fix test syntax
* Tests syntax
* Loose test against actual google api
* Typo
* Typo
* Syntax
* Missing `;`
* Make integration tests against real google api looser
* StaticMaps - Fixed gamma
* Fixed regex escaping
* Ease integration test with google
* Ease integration test with google
* Update README.md
* Update PlacesDetailsTests.cs
* Update PlaceAutocompleteTests.cs
* Typo
* Tests - If no key in config man, take it from env var
* Nuget badge
* Nugetstatus badge

## V 0.42
* Create .nuspec
* Revert "suppress warn 1519 in build"
* Suppress warn 1519 in build
* Merge pull request #42 from kevbite/master
* Adding in a IEngineFacade to allow mocking and verification in consumer tests.
* Travis-ci badge
* Fixed integ test due to new points in test of directions
* Fixed test due to google `Avenue` -> `Ave` change
* Create .travis.yml
* Merge pull request #38 from rossiter10/master
* Added radar search
* Removed files of Autocomplete desktop sample app
* Release notes file
* Release notes file

## V 0.41
* Fixed sub steps for directions (9712e572b89ee2eb552728109e616547ed085b52)
* Added PlaceId (b23882eaba698d049ad4707593de24a127bf3bea)
* The Id and References fields are being deprecated in favor of PlaceId. I've added the PlaceId field. (3162e9acfa821cb2a5ee835a608693faf81e7e47)
* Autocomplete APIs (f57ff14dc4e32f06f917b5b7bf0ce447ad00047a)

## V 0.30.0.0
* Integration tests fixed with actual Google servers, version changed to 0.30.0.0
* Merge pull request #25 from graham128/places-location
* Add the Geometry property to Places Response
* Add the Geometry property to Responses Result
* Add Places\Response\Geometry source
* Merge pull request #24 from Its-Tyson/not-found-status-result
* Moved Places Details response status enum comments to xml doc format for intellisense.
* Added missing Places Details result status code enum values.
* Added unit test reproducing failed status code deserialization.
* Merge pull request #23 from Its-Tyson/places-details-additions
* Added integration test for opening hours.
* Added opening hours to places details query.
* Added Places Details query integration tests.
* Changed PriceLevel to strongly typed enum.
* Added price level, utc offset and website values.
* Arranged members alphabetically.
* Merge pull request #22 from IDisposable/patch-1
* Corrected parameter name for the "location" query string
* Version 0.22.0.0
* Merge pull request #21 from Its-Tyson/places-text-location-results
* Added location lat/long support to PlacesText API results.
* Merge pull request #20 from petelopez/master
* Added partial_match support as requested issue #19
* Fixed the Geometry property XML comments
* Changed code comments to XML comments for API consumers
* Merge remote-tracking branch 'upstream/master'
* Merge github.com:maximnovak/google-maps
* Merge branch 'issue2'
* Fixed issue 2
* Add yet another entry to .gitignore

## V 0.21.0.0
* PlacesDetails.Response.Result added Geometry property
* Nuget
* Version 0.20.0.0
* Visual Studio 2012 / FW 4.5
* Removed the dependency on Microsoft.CompilerServices.AsyncTargetingPack
* Merge pull request #18 from darwindave/master
* Merge pull request #16 from vivet/master
* 0.19.0.0
* Fixed XML docs.
* V 0.18.0.0
* Merge pull request #13 from lucasjans/Places-Details
* Merge pull request #11 from sevagd/master
* Step.TravelMode as auto property
* Merge pull request #10 from Genbox/master
* Moved time conversion to Unix time to class - UnixTimeConverter
* Merge pull request #9 from Genbox/master
* Update README.md
* V 0.17.0.0
* V 0.17.0.0
* OptimizeWaypoints (Issue #6)
* Polylines for each steps (Issue #7)
* Lazy point decoding in Route
* Expose the RAW encoded points as public method.
* Link to documentation of encoding
* Spaces->tabs
* Spaces -> tabs
* Reformat code
* Resharper team settings file
* ILocation -> ILocationString
* Changed internal serialization implementation protection level from public to internal
* Nuget
* Version 0.16
* Integration test for OverviewPath
* QueryStringParametersList changed
* New nuget package 0.15.0.0
* Fixed issue #4
* Removed use of completion handler from synchronous version. - Added parameter validation to WebClientEx constructor.
* Made the default timeout of 100 seconds a static readonly field. - Split a complex line of code for clarity.
* Added timeout support to synchronous API so that synchronous APIs on the GoogleMaps class can be used in a truly synchronous fashion. - Updated GoogleMaps class to use synchronous APIs. - Added more integration tests since the code paths of sync and async have diverged.
* Version changed to 0.15.0.0
* Implemented the Synchronous query
* Fix bug where cancelling a request before the download has started was ignored. - Made DownloadDataTaskAsync() unregister the event handler when done (to prevent memory leaks), in case these extension methods are used outside this library where the WebClient instance may be reused. - Removed Subversion reference from solution file.
* Added the NuGet package Microsoft.CompilerServices.AsyncTargetingPack as a dependency which provides Microsoft's official TaskEx class implementation for .NET 4.0. - Removed the unofficial TaskEx class. - Removed the DownloadDataTaskAsync(WebClient, Uri) overload since it's already provided by AsyncTargetingPack. - Fixed malformed XML comments.
* Added XML docs generation at build time to the 'Release' configuration. The three files that are built by this configuration (.dll, .pdb and .xml) should be used for creating the NuGet package.
* Added support for cancellation. - Added support for specifying a timeout. - Added integrations tests for async operations, timeout and cancellation.
* Fixed "jpg-baselin" to "jpg-baseline" in StaticMapsEngine. - Switched StaticMapsEngine to use QueryStringParametersList. - Made StaticMaps tests use the the AAA pattern (Arrange-Act-Assert).
* Made the GoogleMaps public and added XML docs. - Made GoogleMaps's constructor private. - Marked the existing APIs with [Obsolete]. - Updated unit/integration tests and MapsApiTest to use the GoogleMaps class.
* Merge remote-tracking branch 'git-hub/master'
* Made the methods of `MapsAPIGenericEngine` static. Class can be made static when backwards compatibility is dropped. - Added the internal `GoogleMaps` class with the suggested API. This can be made the primary public surface API if it is acceptable. - Added the` IResponseFor` marker interface to make sure the request/response types match up at compile time (instead of at run time where it would throw an exception). - Added XML doc specifying the default number of connections.
* Updated readme - link to nuget package
* More generic approach for creating query string in StaticMaps
* Merged 8c5ae6c and 09a99a2.
* Merge commit 'a01b1644c3c9ed59fe19166584cde6d046c880e6' (NUnit via NuGet)
* Removed duplicate unused files that were probably mistakenly pushed during the move from Google Code to GitHub. - Changed WebClient async usage to DownloadDataTaskAsync instead of DownloadStringTaskAsync so that an intermediary string isn't created. - Moved the query string creation from the WebClient .QueryString to a virtual method on MapsBaseRequest that returns a QueryStringParametersList. - Moved BaseUrl to MapsBaseRequest class. - Moved the Uri creation to the virtual GetUri() method of MapsBaseRequest class, which returns the full Uri including the query string. - With the above three changes, URL signing was implemented. Added a new abstract class SignableRequest that inherits from MapsBaseRequest, and made all requests except Places (which uses the old API that doesn't support signing) inherit from SignableRequest. If credentials are provided, GetUri will produce a signed Uri. - When a request fails due to providing incorrect credentials, an AuthorizationException will be thrown. - Added the ability to control the number of HTTP and HTTPS connection limit on a per-engine basis, for use in high throughput scenarios. - Added *.user to .gitignore. - Advanced the file version to 0.12.0.0.
* Version to 0.11.0.0
* Merge pull request #1 from petelopez/master
* Initial code from google-code
* Initial commit