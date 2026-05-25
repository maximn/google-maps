using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

string? address = args.Length > 0
    ? string.Join(' ', args)
    : PromptForAddress();

if (string.IsNullOrWhiteSpace(address))
{
    Console.Error.WriteLine("No address provided. Pass it as args or type one when prompted.");
    return 1;
}

string? apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("GOOGLE_API_KEY environment variable is not set.");
    return 2;
}

using var httpClient = new HttpClient();
IGoogleMapsClient maps = new GoogleMapsClient(httpClient, new GoogleMapsClientOptions { ApiKey = apiKey });

var request = new GeocodingRequest { Address = address };

GeocodingResponse response = await maps.Geocode.QueryAsync(request);

if (response.Status != Status.OK || response.Results is null)
{
    Console.Error.WriteLine($"Geocoding failed: {response.Status}");
    return 3;
}

foreach (var result in response.Results)
{
    var location = result.Geometry.Location;
    Console.WriteLine($"Address : {result.FormattedAddress}");
    Console.WriteLine($"Lat/Lng : {location.Latitude:F6}, {location.Longitude:F6}");
    Console.WriteLine();
}

return 0;

static string? PromptForAddress()
{
    Console.Write("Address: ");
    return Console.ReadLine();
}
