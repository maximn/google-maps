using System;
using GoogleMapsApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for registering <see cref="IGoogleMapsClient"/> with an
    /// <see cref="IServiceCollection"/>, wired to <c>IHttpClientFactory</c>.
    /// </summary>
    public static class GoogleMapsServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="IGoogleMapsClient"/> backed by <c>IHttpClientFactory</c>.
        /// No ambient API key is configured — callers must set <c>ApiKey</c> on each request,
        /// or configure options elsewhere.
        /// </summary>
        /// <returns>An <see cref="IHttpClientBuilder"/> for chaining (e.g. resilience handlers).</returns>
        public static IHttpClientBuilder AddGoogleMaps(this IServiceCollection services)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            services.AddOptions<GoogleMapsClientOptions>();
            return AddCore(services);
        }

        /// <summary>
        /// Registers <see cref="IGoogleMapsClient"/> backed by <c>IHttpClientFactory</c> and
        /// configures <see cref="GoogleMapsClientOptions"/> (e.g. the ambient API key).
        /// </summary>
        /// <returns>An <see cref="IHttpClientBuilder"/> for chaining (e.g. resilience handlers).</returns>
        public static IHttpClientBuilder AddGoogleMaps(this IServiceCollection services, Action<GoogleMapsClientOptions> configureOptions)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (configureOptions is null) throw new ArgumentNullException(nameof(configureOptions));

            services.Configure(configureOptions);
            return AddCore(services);
        }

        /// <summary>
        /// Registers <see cref="IGoogleMapsClient"/> backed by <c>IHttpClientFactory</c> and binds
        /// <see cref="GoogleMapsClientOptions"/> from the supplied configuration section.
        /// </summary>
        /// <returns>An <see cref="IHttpClientBuilder"/> for chaining (e.g. resilience handlers).</returns>
        public static IHttpClientBuilder AddGoogleMaps(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            services.Configure<GoogleMapsClientOptions>(configuration);
            return AddCore(services);
        }

        private static IHttpClientBuilder AddCore(IServiceCollection services)
            => services.AddHttpClient<IGoogleMapsClient, GoogleMapsClient>()
                       .AddTypedClient<IGoogleMapsClient>((httpClient, sp) =>
                           new GoogleMapsClient(
                               httpClient,
                               sp.GetRequiredService<IOptions<GoogleMapsClientOptions>>().Value));
    }
}
