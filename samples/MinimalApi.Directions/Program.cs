using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;

var builder = WebApplication.CreateBuilder(args);

// Resolve the API key once at startup and register a singleton GoogleMapsClient.
// In real apps, prefer the GoogleMapsApi.Extensions.DependencyInjection package (ships in 2.1)
// which wraps this with IHttpClientFactory + the Options pattern in one call.
string apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")
                ?? builder.Configuration["GoogleApiKey"]
                ?? throw new InvalidOperationException(
                    "GOOGLE_API_KEY env var or GoogleApiKey config value is required.");

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IGoogleMapsClient>(sp => new GoogleMapsClient(
    sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(GoogleMapsClient)),
    new GoogleMapsClientOptions { ApiKey = apiKey }));

var app = builder.Build();

app.MapGet("/directions", async (string origin, string destination, IGoogleMapsClient maps) =>
{
    if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
    {
        return Results.BadRequest(new { error = "origin and destination are required" });
    }

    var request = new DirectionsRequest { Origin = origin, Destination = destination };

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
