using Samples.Blazor.Geocoding.Components;
using Samples.Blazor.Geocoding.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<GeocodingService>(sp =>
{
    string? apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")
                     ?? builder.Configuration["GoogleApiKey"];
    var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(GeocodingService));
    return new GeocodingService(apiKey, http);
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
