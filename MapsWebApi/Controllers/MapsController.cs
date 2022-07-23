using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using GoogleMapsApi.StaticMaps.Enums;
using MapsWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web.Helpers;

namespace MapsWebApi.Controllers
{
    public class Data
    {
        public string img;
        public string start;
        public string end;
        public string duration;
        public Data(string img, string start, string end, string duration)
        {
            this.img = img;
            this.start = start;
            this.end = end;
            this.duration = duration;
            
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        IConfiguration configuration;
        public MapsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpPost]
        public IActionResult GetMapImage(CustomImageRequest request)
        {
            DirectionsRequest directionsRequest = new DirectionsRequest()
            {
                ApiKey = configuration["Key"],
                Origin = isCoordinates(request.Origin) ? request.Origin : $"{request.City}, {request.Origin}",
                Destination = isCoordinates(request.Destination) ? request.Destination : $"{request.City}, {request.Destination}"
            };

            DirectionsResponse directions = GoogleMaps.Directions.QueryAsync(directionsRequest).Result;

            StaticMapsEngine staticMapGenerator = new StaticMapsEngine();


            IEnumerable<Step> steps = directions.Routes.First().Legs.First().Steps;
            IList<ILocationString> path = steps.Select(step => step.StartLocation)
                                               .ToList<ILocationString>();
            path.Add(steps.Last().EndLocation);

            StaticMapRequest staticMapRequest = new StaticMapRequest(null, default, new ImageSize(1000, 600))
            {
                ApiKey = configuration["Key"],
                Pathes = new List<GoogleMapsApi.StaticMaps.Entities.Path>()
                {
                    new GoogleMapsApi.StaticMaps.Entities.Path()
                    {
                        Style = new PathStyle()
                        {
                            Color = "0x800080",
                            Weight = 4
                        },
                        Locations = path
                    }
                },
                MapType = request.MapType
            };

            string url = staticMapGenerator.GenerateStaticMapURL(staticMapRequest);

            var route = directions.Routes.First().Legs.First();
            return Ok(JsonConvert.SerializeObject(new Data(url, String.Join(",", route.StartAddress.Split(',')[0..3]), String.Join(",", route.EndAddress.Split(',')[0..3]), route.Duration.Text)));
        }
        bool isCoordinates(string value)
        {
            return Double.TryParse(value.Split(',')[0], out _);
        }
    }
}