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
	public class GoogleMaps
	{
		/// <summary>Perform geocoding operations.</summary>
		public static EngineFacade<GeocodingRequest, GeocodingResponse> Geocode
		{
			get
			{
				return EngineFacade<GeocodingRequest, GeocodingResponse>.Instance;
			}
		}
		/// <summary>Perform directions operations.</summary>
		public static EngineFacade<DirectionsRequest, DirectionsResponse> Directions
		{
			get
			{
				return EngineFacade<DirectionsRequest, DirectionsResponse>.Instance;
			}
		}
		/// <summary>Perform elevation operations.</summary>
		public static EngineFacade<ElevationRequest, ElevationResponse> Elevation
		{
			get
			{
				return EngineFacade<ElevationRequest, ElevationResponse>.Instance;
			}
		}
		/// <summary>Perform places operations.</summary>
		public static EngineFacade<PlacesRequest, PlacesResponse> Places
		{
			get
			{
				return EngineFacade<PlacesRequest, PlacesResponse>.Instance;
			}
		}
	}

	/// <summary>
	/// A public-surface API that exposes the Google Maps API functionality.
	/// </summary>
	/// <typeparam name="TRequest"></typeparam>
	/// <typeparam name="TResponse"></typeparam>
	public class EngineFacade<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : IResponseFor<TRequest>
	{
		internal static readonly EngineFacade<TRequest, TResponse> Instance = new EngineFacade<TRequest, TResponse>();

		private EngineFacade() { }

		/// <summary>
		/// Determines the maximum number of concurrent HTTP connections to open to this engine's host address. The default value is 2 connections.
		/// </summary>
		/// <remarks>
		/// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
		/// </remarks>
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
		
		/// <summary>
		/// Determines the maximum number of concurrent HTTPS connections to open to this engine's host address. The default value is 2 connections.
		/// </summary>
		/// <remarks>
		/// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
		/// </remarks>
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

		/// <summary>
		/// Query the Google Maps API using the provided request.
		/// </summary>
		/// <param name="request">The request that will be sent.</param>
		/// <returns>The response that was received.</returns>
		public TResponse Query(TRequest request)
		{
			return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPI(request);
		}
		
		/// <summary>
		/// Asynchronously query the Google Maps API using the provided request.
		/// </summary>
		/// <param name="request">The request that will be sent.</param>
		/// <returns>A task that will produce the response that was received.</returns>
		public Task<TResponse> QueryAsync(TRequest request)
		{
			return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPIAsync(request);
		}
	}
}
