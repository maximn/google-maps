using GoogleMapsApi;
using Samples.Blazor.Geocoding.Components;
using Samples.Blazor.Geocoding.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register the instance-based client with IHttpClientFactory (the recommended pattern).
builder.Services.AddHttpClient<IGoogleMapsClient, GoogleMapsClient>();

builder.Services.AddScoped<GeocodingService>(sp =>
{
    string? apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")
                     ?? builder.Configuration["GoogleApiKey"];
    return new GeocodingService(sp.GetRequiredService<IGoogleMapsClient>(), apiKey);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
