using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;

var builder = WebApplication.CreateBuilder(args);

string? apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")
                 ?? builder.Configuration["GoogleApiKey"];

// Register IGoogleMapsClient (IHttpClientFactory-backed) and the ambient API key in one call.
builder.Services.AddGoogleMaps(options => options.ApiKey = apiKey);

var app = builder.Build();

app.MapGet("/directions", async (string origin, string destination, IGoogleMapsClient maps) =>
{
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        return Results.Problem(
            "GOOGLE_API_KEY env var or GoogleApiKey config value is required.",
            statusCode: StatusCodes.Status500InternalServerError);
    }

    if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
    {
        return Results.BadRequest(new { error = "origin and destination are required" });
    }

    // No per-request ApiKey needed — it's auto-filled from the ambient options above.
    var request = new DirectionsRequest
    {
        Origin = origin,
        Destination = destination,
    };

    DirectionsResponse response = await maps.Directions.QueryAsync(request);

    if (response.Status != DirectionsStatusCodes.OK || response.Routes is null)
    {
        return Results.Problem(
            $"Directions failed: {response.Status} {response.ErrorMessage}",
            statusCode: StatusCodes.Status502BadGateway);
    }

    var route = response.Routes.First();
    var leg = route.Legs?.FirstOrDefault();

    return Results.Ok(new
    {
        summary = route.Summary,
        startAddress = leg?.StartAddress,
        endAddress = leg?.EndAddress,
        distance = leg?.Distance?.Text,
        duration = leg?.Duration?.Text,
        copyrights = route.Copyrights,
    });
});

app.Run();
