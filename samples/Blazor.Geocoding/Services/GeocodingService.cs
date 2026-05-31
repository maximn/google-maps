using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

namespace Samples.Blazor.Geocoding.Services;

public sealed class GeocodingService
{
    private readonly IGoogleMapsClient _maps;
    private readonly string? _apiKey;

    public GeocodingService(IGoogleMapsClient maps, string? apiKey)
    {
        _maps = maps;
        _apiKey = apiKey;
    }

    public bool HasApiKey => !string.IsNullOrWhiteSpace(_apiKey);

    public async Task<GeocodingResponse> GeocodeAsync(string address, CancellationToken ct = default)
    {
        var request = new GeocodingRequest
        {
            Address = address,
            ApiKey = _apiKey,
        };

        return await _maps.Geocode.QueryAsync(request, ct);
    }
}
