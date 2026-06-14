using System.Net.Http;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.AerialView.Request;
using GoogleMapsApi.Entities.AerialView.Response;

namespace GoogleMapsApi
{
    /// <summary>
    /// Default <see cref="IAerialViewApi"/> implementation grouping the two Aerial View endpoints over a
    /// shared <see cref="HttpClient"/> and <see cref="GoogleMapsClientOptions"/>.
    /// </summary>
    internal sealed class AerialViewApi : IAerialViewApi
    {
        public AerialViewApi(HttpClient httpClient, GoogleMapsClientOptions options)
        {
            RenderVideo = new HttpClientEngineFacade<RenderVideoRequest, AerialViewVideoResponse>(httpClient, options);
            LookupVideo = new HttpClientEngineFacade<LookupVideoRequest, AerialViewVideoResponse>(httpClient, options);
        }

        /// <inheritdoc/>
        public IEngineFacade<RenderVideoRequest, AerialViewVideoResponse> RenderVideo { get; }

        /// <inheritdoc/>
        public IEngineFacade<LookupVideoRequest, AerialViewVideoResponse> LookupVideo { get; }
    }
}
