namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{
    using System;
    using System.Globalization;

    using Common;

    using GoogleMapsApi.Entities.Directions.Request;
    using System.Linq;

    using GoogleMapsApi.Engine;

    public class DistanceMatrixRequest : SignableRequest
    {
        protected internal override string BaseUrl
        {
            get
            {
                return base.BaseUrl + "distancematrix/";
            }
        }

        public string[] Origins { get; set; }

        public string[] Destinations { get; set; }

        /// <summary>
		/// alternatives (optional), if set to true, specifies that the Directions service may provide more than one route alternative in the response. Note that providing route alternatives may increase the response time from the server.
		/// </summary>
		public bool Alternatives { get; set; }

        /// <summary>
		///  The desired time of departure. 
		/// </summary>
		public Time DepartureTime { get; set; }

        protected override QueryStringParametersList GetQueryStringParameters()
        {
            if (Origins == null || !Origins.Any())
                throw new ArgumentException("Must specify an Origins");
            if (Destinations == null || !Destinations.Any())
                throw new ArgumentException("Must specify a Destinations");

            var parameters = base.GetQueryStringParameters();
            parameters.Add("origins", string.Join("|", Origins));
            parameters.Add("destinations", string.Join("|", Destinations));

            if (DepartureTime != null)
                parameters.Add("departure_time", DepartureTime.ToString());

            return parameters;
        }
    }
}
