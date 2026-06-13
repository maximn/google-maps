using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;

namespace GoogleMapsApi.Benchmarks;

/// <summary>
/// Benchmarks the network-free request-URL composition path (<c>MapsBaseRequest.GetUri()</c>
/// and the Static Maps engine) across the .NET 8 and .NET 10 runtimes.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class UrlGenerationBenchmarks
{
    private const string ApiKey = "benchmark-dummy-key";

    private GeocodingRequest _geocoding = null!;
    private DirectionsRequest _directions = null!;
    private DirectionsRequest _directionsWithWaypoints = null!;
    private DistanceMatrixRequest _distanceMatrix = null!;
    private ElevationRequest _elevation = null!;
    private TimeZoneRequest _timeZone = null!;
    private StaticMapsEngine _staticMapsEngine = null!;
    private StaticMapRequest _staticMap = null!;

    [GlobalSetup]
    public void Setup()
    {
        _geocoding = new GeocodingRequest
        {
            Address = "1600 Amphitheatre Parkway, Mountain View, CA",
            ApiKey = ApiKey,
        };

        _directions = new DirectionsRequest
        {
            Origin = "Mountain View, CA",
            Destination = "San Francisco, CA",
            TravelMode = TravelMode.Driving,
            ApiKey = ApiKey,
        };

        _directionsWithWaypoints = new DirectionsRequest
        {
            Origin = "Mountain View, CA",
            Destination = "San Francisco, CA",
            Waypoints = new[] { "Palo Alto, CA", "San Mateo, CA", "Daly City, CA" },
            OptimizeWaypoints = true,
            TravelMode = TravelMode.Driving,
            ApiKey = ApiKey,
        };

        _distanceMatrix = new DistanceMatrixRequest
        {
            Origins = new[] { "Mountain View, CA", "Oakland, CA" },
            Destinations = new[] { "San Francisco, CA", "Fresno, CA" },
            ApiKey = ApiKey,
        };

        _elevation = new ElevationRequest
        {
            Locations = new[]
            {
                new Location(37.4224764, -122.0842499),
                new Location(37.7749295, -122.4194155),
            },
            ApiKey = ApiKey,
        };

        _timeZone = new TimeZoneRequest
        {
            Location = new Location(37.4224764, -122.0842499),
            TimeStamp = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ApiKey = ApiKey,
        };

        _staticMapsEngine = new StaticMapsEngine();
        _staticMap = new StaticMapRequest(
            new AddressLocation("Times Square, New York, NY"),
            zoom: 13,
            imageSize: new ImageSize(500, 400))
        {
            ApiKey = ApiKey,
            IsSSL = true,
        };
    }

    [Benchmark]
    public Uri Geocoding() => _geocoding.GetUri();

    [Benchmark]
    public Uri Directions() => _directions.GetUri();

    [Benchmark]
    public Uri Directions_WithWaypoints() => _directionsWithWaypoints.GetUri();

    [Benchmark]
    public Uri DistanceMatrix() => _distanceMatrix.GetUri();

    [Benchmark]
    public Uri Elevation() => _elevation.GetUri();

    [Benchmark]
    public Uri TimeZone() => _timeZone.GetUri();

    [Benchmark]
    public string StaticMap() => _staticMapsEngine.GenerateStaticMapURL(_staticMap);
}
