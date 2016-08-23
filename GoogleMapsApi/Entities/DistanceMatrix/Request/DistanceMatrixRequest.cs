namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{
    using System;
    using System.Globalization;

    using Common;

    using GoogleMapsApi.Entities.DistanceMatrix.Request;
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

        /// <summary>
		///  The desired time of arrival. 
		/// </summary>
		public Time ArrivalTime { get; set; }

        public DistanceMatrixTravelModes? Mode { get; set; }

        public DistanceMatrixTrafficModels? TrafficModel { get; set; }

        public DistanceMatrixTransitRoutingPreferences? TransitRoutingPreference { get; set; }

        public DistanceMatrixRestrictions? Avoid { get; set; }

        public DistanceMatrixUnitSystems? Units { get; set; }

        public string Language { get; set; }

        protected override QueryStringParametersList GetQueryStringParameters()
        {
            if (Origins == null || !Origins.Any())
                throw new ArgumentException("Must specify an Origins");
            if (Destinations == null || !Destinations.Any())
                throw new ArgumentException("Must specify a Destinations");
            if (DepartureTime != null && ArrivalTime != null)
                throw new ArgumentException("Must not specify both an ArrivalTime and a DepartureTime");
            if (Mode != DistanceMatrixTravelModes.transit && ArrivalTime != null)
                throw new ArgumentException("Must not specify ArrivalTime for modes other than Transit");
            if (Mode != DistanceMatrixTravelModes.transit && TransitRoutingPreference != null)
                throw new ArgumentException("Must not specify TransitRoutingPreference for modes other than Transit");
            if ((!(Mode == null || Mode == DistanceMatrixTravelModes.driving) || DepartureTime == null)
                && TrafficModel != null)
                throw new ArgumentException("A TrafficModel must not be specified unless the Mode is Driving and a DepartureTime is provided");

            var parameters = base.GetQueryStringParameters();
            parameters.Add("origins", string.Join("|", Origins));
            parameters.Add("destinations", string.Join("|", Destinations));

            if (DepartureTime != null)
                parameters.Add("departure_time", DepartureTime.ToString());

            if (ArrivalTime != null)
                parameters.Add("arrival_time", ArrivalTime.ToString());

            if (Mode != null)
                parameters.Add("mode", Mode.ToString());

            if (TrafficModel != null)
                parameters.Add("traffic_model", TrafficModel.ToString());

            if (TransitRoutingPreference != null)
                parameters.Add("transit_routing_preference", TransitRoutingPreference.ToString());

            if (Avoid != null)
                parameters.Add("avoid", Avoid.ToString());

            if (Units != null)
                parameters.Add("units", Units.ToString());

            if (!string.IsNullOrWhiteSpace(Language))
                parameters.Add("language", Language);

            return parameters;
        }
    }
}
