using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi
{
    /// <summary>
    /// This class is to bridge from the old WebClientExtensionMethods.cs
    /// Because tasks naturally support behavior given in this class, it is better to use tasks directly than to use this class
    /// </summary>
    [Obsolete]
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Constant. Specified an infinite timeout duration. This is a TimeSpan of negative one (-1) milliseconds.
        /// </summary>
        [Obsolete]
        public static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1);

        /// <summary>
        /// Asynchronously downloads the resource with the specified URI as a Byte array limited by the specified timeout and allows cancelling the operation.
        /// </summary>
        /// <param name="client">The client with which to download the specified resource.</param>
        /// <param name="address">The address of the resource to download.</param>
        /// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
        /// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
        /// When a request is aborted due to a timeout the returned task will transition to the Faulted state with a TimeoutException.</param>
        /// <param name="token">A cancellation token that can be used to cancel the pending asynchronous task.</param>
        /// <returns>A Task with the future value of the downloaded string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null value is passed to the client or address parameters.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value of timeout is neither a positive value or infinite.</exception>
        [Obsolete]
        public static async Task<byte[]> DownloadDataTaskAsync(this HttpClient client, Uri address, TimeSpan timeout, CancellationToken token = new CancellationToken())
        {
            var dataDownloaded = await DownloadData(client, address, timeout, token).ConfigureAwait(false);

            if (dataDownloaded != null)
                return await dataDownloaded.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            return await GetCancelledTask<byte[]>().ConfigureAwait(false);
        }

        public static async Task<string> DownloadDataTaskAsyncAsString(this HttpClient client, Uri address, TimeSpan timeout, CancellationToken token = new CancellationToken())
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(timeout);
                
                HttpResponseMessage response = await client.GetAsync(address, cts.Token).ConfigureAwait(false);
                await HandleResponse(response, timeout);
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        private static async Task<T> GetCancelledTask<T>()
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetCanceled();
            return await tcs.Task.ConfigureAwait(false);
        }

        private static async Task<HttpResponseMessage> DownloadData(this HttpClient client, Uri address, TimeSpan timeout, CancellationToken token = new CancellationToken())
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (address == null) throw new ArgumentNullException(nameof(address));
            if (timeout.TotalMilliseconds < 0 && timeout != InfiniteTimeout)
                throw new ArgumentOutOfRangeException("address", timeout, "The timeout value must be a positive or equal to InfiniteTimeout.");

            if (token.IsCancellationRequested)
                return null;

            var httpResponse = await client.GetAsync(address, token).ConfigureAwait(false);
            await HandleResponse(httpResponse, timeout);
            return httpResponse;
        }

        private static async Task HandleResponse(HttpResponseMessage response, TimeSpan timeout)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.ProxyAuthenticationRequired ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new AuthenticationException(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                if (response.StatusCode == HttpStatusCode.GatewayTimeout ||
                    response.StatusCode == HttpStatusCode.RequestTimeout)
                    throw new TimeoutException($"The request has exceeded the timeout limit of {timeout} and has been aborted.");

                throw new HttpRequestException($"Failed with HttpResponse: {response.StatusCode} and message: {response.ReasonPhrase}");
            }
        }
    }
}