using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using GoogleMapsApi.Common;

namespace GoogleMapsApi.Elevation.Response
{
    [DataContract]
    public class Result
    {
        /// <summary>
        /// A location element (containing lat and lng elements) of the position for which elevation data is being computed. Note that for path requests, the set of location elements will contain the sampled points along the path.
        /// </summary>
        [DataMember(Name="location")]
        public Location Location { get; set; }

        /// <summary>
        /// An elevation element indicating the elevation of the location in meters.
        /// </summary>
        [DataMember(Name = "elevation")]
        public double Elevation { get; set; }
    }
}
