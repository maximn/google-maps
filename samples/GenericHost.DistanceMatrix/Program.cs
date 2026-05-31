using GoogleMapsApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Samples.GenericHost.DistanceMatrix;

var builder = Host.CreateApplicationBuilder(args);

// Keep the repo-wide GOOGLE_API_KEY convention working by surfacing it into the
// "GoogleMaps" section that AddGoogleMaps(IConfiguration) binds below.
var envApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
if (!string.IsNullOrWhiteSpace(envApiKey))
{
    builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["GoogleMaps:ApiKey"] = envApiKey,
    });
}

// Register IGoogleMapsClient (IHttpClientFactory-backed) and bind GoogleMapsClientOptions
// from the "GoogleMaps" config section — appsettings.json, environment variables and
// user secrets all flow through here, so the ambient API key lives in configuration.
builder.Services.AddGoogleMaps(builder.Configuration.GetSection("GoogleMaps"));

// A typical application service that depends on IGoogleMapsClient by constructor injection.
builder.Services.AddSingleton<TravelTimeService>();

using var host = builder.Build();

var origin = args.ElementAtOrDefault(0) ?? "New York, NY";
var destination = args.ElementAtOrDefault(1) ?? "Boston, MA";

var travelTime = host.Services.GetRequiredService<TravelTimeService>();
return await travelTime.RunAsync(origin, destination);
