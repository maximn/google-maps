using GoogleMapsApi.Entities.Common;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.Engine
{
    /// <summary>
    /// Per-instance engine facade backed by a caller-supplied <see cref="HttpClient"/> and ambient
    /// <see cref="GoogleMapsClientOptions"/>. Auto-fills the request's <c>ApiKey</c> from the options
    /// when the caller has not set one. Events are scoped to this instance only.
    /// </summary>
    internal sealed class HttpClientEngineFacade<TRequest, TResponse> : IEngineFacade<TRequest, TResponse>
        where TRequest : MapsBaseRequest, new()
        where TResponse : IResponseFor<TRequest>
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleMapsClientOptions _options;

        public HttpClientEngineFacade(HttpClient httpClient, GoogleMapsClientOptions options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public event UriCreatedDelegate? OnUriCreated;
        public event RawResponseReceivedDelegate? OnRawResponseReceived;

        public Task<TResponse> QueryAsync(TRequest request)
            => QueryAsync(request, TimeSpan.FromMilliseconds(Timeout.Infinite), CancellationToken.None);

        public Task<TResponse> QueryAsync(TRequest request, TimeSpan timeout)
            => QueryAsync(request, timeout, CancellationToken.None);

        public Task<TResponse> QueryAsync(TRequest request, CancellationToken token)
            => QueryAsync(request, TimeSpan.FromMilliseconds(Timeout.Infinite), token);

        public Task<TResponse> QueryAsync(TRequest request, TimeSpan timeout, CancellationToken token)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // The engine builds the URI from request.ApiKey synchronously, before its first await.
            // We briefly assign the ambient key, let the engine capture the URI, then restore the
            // caller's original value so the request object the caller passed in is left untouched.
            var shouldRestoreApiKey = string.IsNullOrEmpty(request.ApiKey) && !string.IsNullOrEmpty(_options.ApiKey);
            if (shouldRestoreApiKey)
                request.ApiKey = _options.ApiKey;

            try
            {
                return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPIAsync(
                    _httpClient,
                    request,
                    timeout,
                    token,
                    OnUriCreated,
                    OnRawResponseReceived);
            }
            finally
            {
                if (shouldRestoreApiKey)
                    request.ApiKey = null;
            }
        }
    }
}
