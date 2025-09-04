using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.PlacesFind.Response
{
    /// <summary>
    /// Contains the location
    /// </summary>
    public class Geometry
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; }
    }
}
