using GoogleMapsApi;
using GoogleMapsApi.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using Microsoft.Extensions.Logging;

namespace Samples.GenericHost.DistanceMatrix;

/// <summary>
/// Computes travel distance and duration between two places. The Google Maps client is
/// supplied by dependency injection — this class never touches the API key or HttpClient.
/// </summary>
public sealed class TravelTimeService
{
    private readonly IGoogleMapsClient _maps;
    private readonly ILogger<TravelTimeService> _logger;

    public TravelTimeService(IGoogleMapsClient maps, ILogger<TravelTimeService> logger)
    {
        _maps = maps;
        _logger = logger;
    }

    public async Task<int> RunAsync(string origin, string destination, CancellationToken ct = default)
    {
        // No ApiKey on the request — it's filled from the ambient options bound in DI.
        var request = new DistanceMatrixRequest
        {
            Origins = [origin],
            Destinations = [destination],
            Units = DistanceMatrixUnitSystems.metric,
        };

        DistanceMatrixResponse response = await _maps.DistanceMatrix.QueryAsync(request, ct);

        if (response.Status != DistanceMatrixStatusCodes.OK)
        {
            _logger.LogError("Distance Matrix request failed: {Status} {Error}", response.Status, response.ErrorMessage);
            return 1;
        }

        var element = response.Rows.First().Elements.First();
        if (element.Status != DistanceMatrixElementStatusCodes.OK)
        {
            _logger.LogError("No route from {Origin} to {Destination}: {Status}", origin, destination, element.Status);
            return 1;
        }

        _logger.LogInformation(
            "{Origin} -> {Destination}: {Distance}, about {Duration}",
            response.OriginAddresses.First(),
            response.DestinationAddresses.First(),
            element.Distance.Text,
            element.Duration.Text);

        return 0;
    }
}
