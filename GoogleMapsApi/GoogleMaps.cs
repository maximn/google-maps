using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using GoogleMapsApi.Entities.PlacesText.Request;
using GoogleMapsApi.Entities.PlacesText.Response;

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

        /// <summary>Perform places text search operations.</summary>
        public static EngineFacade<PlacesTextRequest, PlacesTextResponse> PlacesText
        {
            get
            {
                return EngineFacade<PlacesTextRequest, PlacesTextResponse>.Instance;
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
		/// Query the Google Maps API using the provided request with the default timeout of 100,000 milliseconds (100 seconds).
		/// </summary>
		/// <param name="request">The request that will be sent.</param>
		/// <returns>The response that was received.</returns>
		/// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
		/// <exception cref="AggregateException">Thrown when an exception occurs during the operation. See the AggregateException's InnerException for more details.</exception>
		public TResponse Query(TRequest request)
		{
			return Query(request, MapsAPIGenericEngine<TRequest, TResponse>.DefaultTimeout);
		}

		/// <summary>
		/// Query the Google Maps API using the provided request.
		/// </summary>
		/// <param name="request">The request that will be sent.</param>
		/// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
		/// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
		/// When a request is aborted due to a timeout an AggregateException will be thrown with an InnerException of type TimeoutException.</param>
		/// <returns>The response that was received.</returns>
		/// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
		/// <exception cref="AggregateException">Thrown when an exception occurs during the operation. See the AggregateException's InnerException for more details.</exception>
		public TResponse Query(TRequest request, TimeSpan timeout)
		{
			return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPI(request, timeout);
		}

		/// <summary>
		/// Asynchronously query the Google Maps API using the provided request.
		/// </summary>
		/// <param name="request">The request that will be sent.</param>
		/// <returns>A Task with the future value of the response.</returns>
		/// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
		public Task<TResponse> QueryAsync(TRequest request)
		{
			return QueryAsync(request, CancellationToken.None);
		}

		/// <summary>
		/// Asynchronously query the Google Maps API using the provided request.
		/// </summary>
		/// <param name="request">The request that will be sent.</param>
		/// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
		/// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
		/// When a request is aborted due to a timeout the returned task will transition to the Faulted state with a TimeoutException.</param>
		/// <returns>A Task with the future value of the response.</returns>
		/// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when the value of timeout is neither a positive value or infinite.</exception>
		public Task<TResponse> QueryAsync(TRequest request, TimeSpan timeout)
		{
			return QueryAsync(request, timeout, CancellationToken.None);
		}

		/// <summary>
		/// Asynchronously query the Google Maps API using the provided request.
		/// </summary>
		/// <param name="request">The request that will be sent.</param>
		/// <param name="token">A cancellation token that can be used to cancel the pending asynchronous task.</param>
		/// <returns>A Task with the future value of the response.</returns>
		/// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
		public Task<TResponse> QueryAsync(TRequest request, CancellationToken token)
		{
			return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPIAsync(request, TimeSpan.FromMilliseconds(Timeout.Infinite), token);
		}

		/// <summary>
		/// Asynchronously query the Google Maps API using the provided request.
		/// </summary>
		/// <param name="request">The request that will be sent.</param>
		/// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
		/// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
		/// When a request is aborted due to a timeout the returned task will transition to the Faulted state with a TimeoutException.</param>
		/// <param name="token">A cancellation token that can be used to cancel the pending asynchronous task.</param>
		/// <returns>A Task with the future value of the response.</returns>
		/// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when the value of timeout is neither a positive value or infinite.</exception>
		public Task<TResponse> QueryAsync(TRequest request, TimeSpan timeout, CancellationToken token)
		{
			return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPIAsync(request, timeout, token);
		}
	}
}
