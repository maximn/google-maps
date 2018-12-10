using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesFind.Request;

namespace GoogleMapsApi.Entities.PlacesFind.Response
{
    [DataContract]
    public class PlacesFindResponse : IResponseFor<PlacesFindRequest>
    {
        public Status Status { get; set; }

        [DataMember(Name = "status")]
        internal string StatusStr
        {
            get
            {
                return Status.ToString();
            }
            set
            {
                Status = (Status)Enum.Parse(typeof(Status), value);
            }
        }

        /// <summary>
        /// Collection of places. Each result contains only the data types that were specified using the fields parameter, plus html_attributions.
        /// </summary>
        [DataMember(Name = "candidates")]
        public IEnumerable<Candidate> Candidates { get; set; }
    }
}
