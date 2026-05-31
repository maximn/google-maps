using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;

var builder = WebApplication.CreateBuilder(args);

// Register the instance-based client with IHttpClientFactory (the recommended pattern).
builder.Services.AddHttpClient<IGoogleMapsClient, GoogleMapsClient>();

var app = builder.Build();

app.MapGet("/directions", async (string origin, string destination, IGoogleMapsClient maps, IConfiguration config) =>
{
    string? apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")
                     ?? config["GoogleApiKey"];

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

    var request = new DirectionsRequest
    {
        Origin = origin,
        Destination = destination,
        ApiKey = apiKey,
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
