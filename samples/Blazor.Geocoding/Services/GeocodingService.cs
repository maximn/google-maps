using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

namespace Samples.Blazor.Geocoding.Services;

public sealed class GeocodingService
{
    private readonly IGoogleMapsClient? _maps;

    public GeocodingService(string? apiKey, HttpClient httpClient)
    {
        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            _maps = new GoogleMapsClient(httpClient, new GoogleMapsClientOptions { ApiKey = apiKey });
        }
    }

    public bool HasApiKey => _maps is not null;

    public Task<GeocodingResponse> GeocodeAsync(string address, CancellationToken ct = default)
    {
        if (_maps is null) throw new InvalidOperationException("API key not configured.");
        return _maps.Geocode.QueryAsync(new GeocodingRequest { Address = address }, ct);
    }
}
