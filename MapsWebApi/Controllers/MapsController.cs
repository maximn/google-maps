using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MapsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMapImage(string firstAddress, string secondAddress)
        {
            DirectionsRequest directionsRequest = new DirectionsRequest()
            {
                ApiKey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI",
                Origin = firstAddress,
                Destination = secondAddress
            };

            DirectionsResponse directions = GoogleMaps.Directions.QueryAsync(directionsRequest).Result;
            Console.WriteLine(directions);

            //GeocodingRequest geocodeRequest = new GeocodingRequest()
            //{
            //    ApiKey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI",
            //    Address = "odessa",
            //};
            //var geocodingEngine = GoogleMaps.Geocode;
            //GeocodingResponse geocode = geocodingEngine.QueryAsync(geocodeRequest).Result;
            //Console.WriteLine(geocode);

            StaticMapsEngine staticMapGenerator = new StaticMapsEngine();

            IEnumerable<Step> steps = directions.Routes.First().Legs.First().Steps;
            IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
            path.Add(steps.Last().EndLocation);

            string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(null, 12, new ImageSize(800, 400))
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

            return Ok(url);
        }
    }
}