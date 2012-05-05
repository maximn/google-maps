using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using GoogleMapsApi.Common;

namespace GoogleMapsApi.Places.Response
{
    [DataContract]
    public class Result
    {
        /// <summary>
        /// name contains the human-readable name for the returned result. For establishment results, this is usually the canonicalized business name.
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name = "vicinity")]
        public string Vicinity { get; set; }

        [DataMember(Name = "type")]
        public LocationType Type { get; set; }

        [DataMember(Name = "formatted_phone_number")]
        public string FormattedPhoneNumber  { get; set; }
    }
}
