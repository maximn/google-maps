using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

var builder = WebApplication.CreateBuilder(args);

string? apiKey = builder.Configuration["GoogleApiKey"]
                 ?? Environment.GetEnvironmentVariable("GOOGLE_API_KEY");

// Register IGoogleMapsClient (IHttpClientFactory-backed) and the ambient API key in one call.
builder.Services.AddGoogleMaps(options => options.ApiKey = apiKey);

var app = builder.Build();

app.MapGet("/geocode", async (string address, IGoogleMapsClient maps) =>
{
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        return Results.Problem(
            "Set your key in appsettings.Development.json (GoogleApiKey) or the GOOGLE_API_KEY env var.",
            statusCode: StatusCodes.Status500InternalServerError);
    }

    if (string.IsNullOrWhiteSpace(address))
    {
        return Results.BadRequest(new { error = "address is required" });
    }

    // The ambient ApiKey from the options above is auto-filled into the request.
    var response = await maps.Geocode.QueryAsync(new GeocodingRequest { Address = address });

    if (response.Status != Status.OK || response.Results is null)
    {
        return Results.Problem(
            $"Geocoding failed: {response.Status}",
            statusCode: StatusCodes.Status502BadGateway);
    }

    return Results.Ok(response.Results.Select(result => new
    {
        formattedAddress = result.FormattedAddress,
        latitude = result.Geometry.Location.Latitude,
        longitude = result.Geometry.Location.Longitude,
        placeId = result.PlaceId,
        types = result.Types,
    }));
});

app.MapGet("/directions", async (string origin, string destination, IGoogleMapsClient maps) =>
{
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        return Results.Problem(
            "Set your key in appsettings.Development.json (GoogleApiKey) or the GOOGLE_API_KEY env var.",
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
    };

    var response = await maps.Directions.QueryAsync(request);

    if (response.Status != GoogleMapsApi.Entities.Directions.Response.DirectionsStatusCodes.OK
        || response.Routes is null)
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
