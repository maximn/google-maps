using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesText.Request;

namespace GoogleMapsApi.Entities.PlacesText.Response
{
    [DataContract]
    public class PlacesTextResponse : IResponseFor<PlacesTextRequest>
    {
        /// <summary>
        /// "status" contains metadata on the request.
        /// </summary>
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
        /// "results" contains an array of places, with information about the place. See Place Search Results for information about these results. The Places API returns up to 20 establishment results. Additionally, political results may be returned which serve to identify the area of the request.
        /// </summary>
        [DataMember(Name = "results")]
        public IEnumerable<Result> Results { get; set; }

        /// <summary>
        /// Contains a token that can be used to return up to 20 additional results. When a next_page_token is returned, it contains the next set of results.
        /// </summary>
        [DataMember(Name = "next_page_token")]
        public string NextPage { get; set; }
    }
}
