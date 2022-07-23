using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;

DirectionsRequest directionsRequest = new DirectionsRequest()
{
    Origin = "NYC, 5th and 39",
    Destination = "Philladephia, Chesnut and Wallnut",
    ApiKey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI"
};

DirectionsResponse directions = GoogleMaps.Directions.QueryAsync(directionsRequest).Result;
Console.WriteLine(directions);

GeocodingRequest geocodeRequest = new GeocodingRequest()
{
    ApiKey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI",
    Address = "new york city",
};
var geocodingEngine = GoogleMaps.Geocode;
GeocodingResponse geocode = geocodingEngine.QueryAsync(geocodeRequest).Result;
Console.WriteLine(geocode);

StaticMapsEngine staticMapGenerator = new StaticMapsEngine();

IEnumerable<Step> steps = directions.Routes.First().Legs.First().Steps;
IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
path.Add(steps.Last().EndLocation);

string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(new Location(40.38742, -74.55366), 9, new ImageSize(800, 400))
{
    ApiKey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI",
    Pathes = new List<GoogleMapsApi.StaticMaps.Entities.Path>(){ new GoogleMapsApi.StaticMaps.Entities.Path()
    {
            Style = new PathStyle()
            {
                    Color = "red"
            },
            Locations = path
    }}
});

Console.WriteLine("Map with path: " + url);