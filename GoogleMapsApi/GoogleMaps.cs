using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Elevation.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;

namespace GoogleMapsApi
{
	// Suggested public-surface API.
	internal class GoogleMaps
	{
		public static EngineFacade<GeocodingRequest, GeocodingResponse> Geocode
		{
			get
			{
				return EngineFacade<GeocodingRequest, GeocodingResponse>.Instance;
			}
		}
		public static EngineFacade<DirectionsRequest, DirectionsResponse> Directions
		{
			get
			{
				return EngineFacade<DirectionsRequest, DirectionsResponse>.Instance;
			}
		}
		public static EngineFacade<ElevationRequest, ElevationResponse> Elevation
		{
			get
			{
				return EngineFacade<ElevationRequest, ElevationResponse>.Instance;
			}
		}
		public static EngineFacade<PlacesRequest, PlacesResponse> Places
		{
			get
			{
				return EngineFacade<PlacesRequest, PlacesResponse>.Instance;
			}
		}
	}

	public class EngineFacade<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : IResponseFor<TRequest>
	{
		internal static readonly EngineFacade<TRequest, TResponse> Instance = new EngineFacade<TRequest, TResponse>();

		public int HttpConnectionLimit
		{
			get
			{
				return MapsAPIGenericEngine<TRequest, TResponse>.HttpConnectionLimit;
			}
			set
			{
				MapsAPIGenericEngine<TRequest, TResponse>.HttpConnectionLimit = value;
			}
		}
		public int HttpsConnectionLimit
		{
			get
			{
				return MapsAPIGenericEngine<TRequest, TResponse>.HttpsConnectionLimit;
			}
			set
			{
				MapsAPIGenericEngine<TRequest, TResponse>.HttpsConnectionLimit = value;
			}
		}
		public TResponse Query(TRequest request)
		{
			return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPI(request);
		}
		public Task<TResponse> QueryAsync(TRequest request)
		{
			return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPIAsync(request);
		}
	}
}
