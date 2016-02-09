using System;
using System.Net;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi
{
    /// <summary>
    /// A public-surface API that exposes the Google Maps API functionality.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class EngineFacade<TRequest, TResponse> : IEngineFacade<TRequest, TResponse>
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
        /// Occurs when the Url created. Can be used for override the Url.
        /// </summary>
        public event UriCreatedDelegate OnUriCreated
        {
            add
            {
                MapsAPIGenericEngine<TRequest, TResponse>.OnUriCreated += value;
            }
            remove
            {
                MapsAPIGenericEngine<TRequest, TResponse>.OnUriCreated -= value;
            }
        }

        /// <summary>
        /// Occurs when raw data from Google API recivied.
        /// </summary>
        public event RawResponseReciviedDelegate OnRawResponseRecivied
        {
            add
            {
                MapsAPIGenericEngine<TRequest, TResponse>.OnRawResponseRecivied += value;
            }
            remove
            {
                MapsAPIGenericEngine<TRequest, TResponse>.OnRawResponseRecivied -= value;
            }
        }

        /// <summary>
        /// Query the Google Maps API using the provided request with the default timeout of 100,000 milliseconds (100 seconds).
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <returns>The response that was received.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        /// <exception cref="AuthenticationException">Thrown when the provided Google client ID or signing key are invalid.</exception>
        /// <exception cref="TimeoutException">Thrown when the operation has exceeded the allotted time.</exception>
        /// <exception cref="WebException">Thrown when an error occurred while downloading data.</exception>
        public TResponse Query(TRequest request)
        {
            return Query(request, MapsAPIGenericEngine<TRequest, TResponse>.DefaultTimeout);
        }

        /// <summary>
        /// Query the Google Maps API using the provided request and timeout period.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
        /// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
        /// When a request is aborted due to a timeout an AggregateException will be thrown with an InnerException of type TimeoutException.</param>
        /// <returns>The response that was received.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        /// <exception cref="AuthenticationException">Thrown when the provided Google client ID or signing key are invalid.</exception>
        /// <exception cref="TimeoutException">Thrown when the operation has exceeded the allotted time.</exception>
        /// <exception cref="WebException">Thrown when an error occurred while downloading data.</exception>
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