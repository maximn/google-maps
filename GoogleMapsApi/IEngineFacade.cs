using GoogleMapsApi.Entities.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        /// Asynchronously query the Google Maps API using the provided request.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <returns>A Task with the future value of the response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        Task<TResponse> QueryAsync(TRequest request);

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
        Task<TResponse> QueryAsync(TRequest request, TimeSpan timeout);

        /// <summary>
        /// Asynchronously query the Google Maps API using the provided request.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="token">A cancellation token that can be used to cancel the pending asynchronous task.</param>
        /// <returns>A Task with the future value of the response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the request parameter.</exception>
        Task<TResponse> QueryAsync(TRequest request, CancellationToken token);

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
        Task<TResponse> QueryAsync(TRequest request, TimeSpan timeout, CancellationToken token);
    }
}