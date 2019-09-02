using GoogleMapsApi.Entities.Common;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesRadar.Response
{
    /// <summary>
    /// Contains the location
    /// </summary>
    [DataContract]
    public class Geometry
    {
        [DataMember(Name = "location")]
        public Location Location { get; set; } 
    }
}
