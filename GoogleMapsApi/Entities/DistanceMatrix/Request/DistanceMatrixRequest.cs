namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{
    using System;
    using System.Globalization;

    using Common;

    using GoogleMapsApi.Entities.DistanceMatrix.Request;
    using System.Linq;

    using GoogleMapsApi.Engine;

    public class DistanceMatrixRequest : SignableRequest, ILocalizableRequest
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

        /// <summary>
        /// For the calculation of distances, you may specify the transportation mode to use. By default, distances are calculated for driving mode. The following travel modes are supported:
        /// - driving(default) indicates distance calculation using the road network.
        /// - walking requests distance calculation for walking via pedestrian paths & sidewalks (where available).
        /// - bicycling requests distance calculation for bicycling via bicycle paths & preferred streets(where available).
        /// - transit requests distance calculation via public transit routes(where available). This value may only be specified if the request includes an API key or a Google Maps APIs Premium Plan client ID.If you set the mode to transit you can optionally specify either a departure_time or an arrival_time.If neither time is specified, the departure_time defaults to now(that is, the departure time defaults to the current time). You can also optionally include a transit_mode and/or a transit_routing_preference.
        /// * Note: Both walking and bicycling routes may sometimes not include clear pedestrian or bicycling paths, so these responses will return warnings in the returned result which you must display to the user.
        /// </summary>
        public DistanceMatrixTravelModes? Mode { get; set; }

        /// <summary>
        /// traffic_model (defaults to best_guess) — Specifies the assumptions to use when calculating time in traffic. This setting affects the value returned in the duration_in_traffic field in the response, which contains the predicted time in traffic based on historical averages. The traffic_model parameter may only be specified for requests where the travel mode is driving, and where the request includes a departure_time, and only if the request includes an API key or a Google Maps APIs Premium Plan client ID. The available values for this parameter are:
        /// - best_guess(default) indicates that the returned duration_in_traffic should be the best estimate of travel time given what is known about both historical traffic conditions and live traffic.Live traffic becomes more important the closer the departure_time is to now.
        /// - pessimistic indicates that the returned duration_in_traffic should be longer than the actual travel time on most days, though occasional days with particularly bad traffic conditions may exceed this value. 
        /// - optimistic indicates that the returned duration_in_traffic should be shorter than the actual travel time on most days, though occasional days with particularly good traffic conditions may be faster than this value.
        /// </summary>
        public DistanceMatrixTrafficModels? TrafficModel { get; set; }

        /// <summary>
        /// transit_routing_preference — Specifies preferences for transit requests. Using this parameter, you can bias the options returned, rather than accepting the default best route chosen by the API. This parameter may only be specified for requests where the mode is transit. The parameter supports the following arguments:
        /// - less_walking indicates that the calculated route should prefer limited amounts of walking.
        /// - fewer_transfers indicates that the calculated route should prefer a limited number of transfers.
        /// </summary>
        public DistanceMatrixTransitRoutingPreferences? TransitRoutingPreference { get; set; }

        /// <summary>
        /// transit_mode — Specifies one or more preferred modes of transit. This parameter may only be specified for requests where the mode is transit. The parameter supports the following arguments:
        /// - bus indicates that the calculated route should prefer travel by bus.
        /// - subway indicates that the calculated route should prefer travel by subway.
        /// - train indicates that the calculated route should prefer travel by train.
        /// - tram indicates that the calculated route should prefer travel by tram and light rail.
        /// - rail indicates that the calculated route should prefer travel by train, tram, light rail, and subway. This is equivalent to transit_mode= train | tram | subway.
        /// </summary>
        public DistanceMatrixTransitModes[] TransitModes { get; set; }

        /// <summary>
        /// Distances may be calculated that adhere to certain restrictions. Restrictions are indicated by use of the avoid parameter, and an argument to that parameter indicating the restriction to avoid. The following restrictions are supported:
        /// tolls, highways, ferries indoor
        /// * Note: the addition of restrictions does not preclude routes that include the restricted feature; it simply biases the result to more favorable routes.
        /// </summary>
        public DistanceMatrixRestrictions? Avoid { get; set; }

        /// <summary>
        /// Distance Matrix results contain text within distance fields to indicate the distance of the calculated route. The unit system to use can be specified:
        /// - metric(default) returns distances in kilometers and meters.
        /// - imperial returns distances in miles and feet.
        /// * Note: this unit system setting only affects the text displayed within distance fields.The distance fields also contain values which are always expressed in meters.
        /// </summary>
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
            if (Mode != DistanceMatrixTravelModes.transit && TransitModes != null && TransitModes.Length > 0)
                throw new ArgumentException("Must not specify TransitModes for modes other than Transit");
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

            if (TransitModes != null && TransitModes.Length > 0)
                parameters.Add("transit_mode", string.Join("|", TransitModes.Select(a => a.ToString())));

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
