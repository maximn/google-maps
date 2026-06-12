using System.Diagnostics;
using System.Reflection;

namespace GoogleMapsApi.Diagnostics
{
    /// <summary>
    /// Distributed-tracing entry point for the library. Every Google Maps API call emits one
    /// <see cref="System.Diagnostics.Activity"/> (an OpenTelemetry span) from the source named
    /// <see cref="SourceName"/>. Subscribe to it from your tracing pipeline — for OpenTelemetry,
    /// call <c>AddSource(GoogleMapsActivity.SourceName)</c> on your <c>TracerProviderBuilder</c>.
    /// Instrumentation is inert (zero allocations) until a listener is registered, so it is always on.
    /// </summary>
    public static class GoogleMapsActivity
    {
        /// <summary>
        /// The <see cref="System.Diagnostics.ActivitySource"/> name spans are emitted from: <c>"GoogleMapsApi"</c>.
        /// Pass this to <c>AddSource(...)</c> to collect the library's traces.
        /// </summary>
        public const string SourceName = "GoogleMapsApi";

        internal static readonly ActivitySource Source =
            new ActivitySource(SourceName, typeof(GoogleMapsActivity).Assembly.GetName().Version?.ToString());
    }
}
