using GoogleMapsApi.Entities.AerialView.Request;
using GoogleMapsApi.Entities.AerialView.Response;

namespace GoogleMapsApi
{
    /// <summary>
    /// Aerial View API operations: render a cinematic flyover video for an address and look up a video's
    /// state and media URIs. Both operations return the same <see cref="AerialViewVideoResponse"/>.
    /// </summary>
    public interface IAerialViewApi
    {
        /// <summary>
        /// Enqueue rendering of a flyover video for a US postal address (free). Returns a
        /// <see cref="VideoState.Processing"/> video to poll via <see cref="LookupVideo"/>, or an
        /// existing video if one is already available.
        /// </summary>
        IEngineFacade<RenderVideoRequest, AerialViewVideoResponse> RenderVideo { get; }

        /// <summary>
        /// Look up a video by id or address (billable). Returns the current state and, once active, the
        /// signed media URIs.
        /// </summary>
        IEngineFacade<LookupVideoRequest, AerialViewVideoResponse> LookupVideo { get; }
    }
}
