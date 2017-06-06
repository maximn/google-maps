using System;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi
{
    using Engine;

    public interface IEngineFacade<in TRequest, TResponse>
        where TRequest : MapsBaseRequest, new()
        where TResponse : IResponseFor<TRequest>
    {
        /// <summary>
        /// Occurs when the Url created. Can be used for override the Url.
        /// </summary>
        event UriCreatedDelegate OnUriCreated;

        /// <summary>
        /// Occurs when raw data from Google API recivied.
        /// </summary>
        event RawResponseReciviedDelegate OnRawResponseRecivied;

        /// <summary>
        /// Query the Google Maps API using the provided request with the default timeout of 100,000 milliseconds (100 seconds).
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <returns>The response that was received.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the provided Google client ID or signing key are invalid.</exception>
        /// <exception cref="TimeoutException">Thrown when the operation has exceeded the allotted time.</exception>
        Task<TResponse> Query(TRequest request);

        /// <summary>
        /// Query the Google Maps API using the provided request and timeout period.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
        /// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
        /// When a request is aborted due to a timeout an AggregateException will be thrown with an InnerException of type TimeoutException.</param>
        /// <returns>The response that was received.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the provided Google client ID or signing key are invalid.</exception>
        /// <exception cref="TimeoutException">Thrown when the operation has exceeded the allotted time.</exception>
        Task<TResponse> Query(TRequest request, TimeSpan timeout);
    }
}