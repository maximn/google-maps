using System.Reflection;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Entities.Geocoding.Response;

namespace GoogleMapsApi.Benchmarks;

/// <summary>
/// Benchmarks the JSON deserialization path the engine actually uses — the production
/// <see cref="JsonSerializerConfiguration"/> options (and their custom converters for enums,
/// <c>Duration</c> and <c>OverviewPolyline</c>) — across the .NET 8 and .NET 10 runtimes.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class JsonDeserializationBenchmarks
{
    private JsonSerializerOptions _options = null!;
    private string _geocodingJson = null!;
    private string _directionsJson = null!;
    private string _distanceMatrixJson = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Exact production options, incl. EnumMember/Duration/OverviewPolyline converters.
        _options = JsonSerializerConfiguration.CreateOptions();

        _geocodingJson = ReadSample("geocoding.json");
        _directionsJson = ReadSample("directions.json");
        _distanceMatrixJson = ReadSample("distancematrix.json");
    }

    private static string ReadSample(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .Single(n => n.EndsWith("SampleData." + fileName, StringComparison.Ordinal));
        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    [Benchmark]
    public GeocodingResponse Geocoding() =>
        JsonSerializer.Deserialize<GeocodingResponse>(_geocodingJson, _options)!;

    [Benchmark]
    public DirectionsResponse Directions() =>
        JsonSerializer.Deserialize<DirectionsResponse>(_directionsJson, _options)!;

    [Benchmark]
    public DistanceMatrixResponse DistanceMatrix() =>
        JsonSerializer.Deserialize<DistanceMatrixResponse>(_distanceMatrixJson, _options)!;
}
