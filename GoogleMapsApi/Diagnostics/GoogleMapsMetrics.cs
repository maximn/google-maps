using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace GoogleMapsApi.Diagnostics
{
    /// <summary>
    /// Metrics entry point for the library. Every Google Maps API call records to a small set of
    /// instruments published from the <see cref="System.Diagnostics.Metrics.Meter"/> named
    /// <see cref="MeterName"/>. Subscribe to it from your metrics pipeline — for OpenTelemetry,
    /// call <c>AddMeter(GoogleMapsMetrics.MeterName)</c> on your <c>MeterProviderBuilder</c>.
    /// Instrumentation is inert (no measurements taken) until a listener subscribes, so it is always on.
    /// </summary>
    /// <remarks>
    /// Three instruments are published, all tagged with <c>gmaps.api</c> (the service name, e.g.
    /// <c>Geocoding</c>):
    /// <list type="bullet">
    /// <item><description><c>gmaps.client.requests</c> — counter of every call attempted (success or failure).</description></item>
    /// <item><description><c>gmaps.client.request.duration</c> — histogram of call latency in seconds.</description></item>
    /// <item><description><c>gmaps.client.request.errors</c> — counter of calls that threw (transport/HTTP failures).</description></item>
    /// </list>
    /// Google "business" errors (for example <c>ZERO_RESULTS</c> or <c>OVER_QUERY_LIMIT</c>) arrive as a
    /// <c>200 OK</c> and do not throw, so they are not counted as errors; they are visible through the
    /// <c>gmaps.response_status</c> tag on the duration histogram.
    /// </remarks>
    public static class GoogleMapsMetrics
    {
        /// <summary>
        /// The <see cref="System.Diagnostics.Metrics.Meter"/> name instruments are published from:
        /// <c>"GoogleMapsApi"</c> (the same name as the tracing source). Pass this to <c>AddMeter(...)</c>
        /// to collect the library's metrics.
        /// </summary>
        public const string MeterName = "GoogleMapsApi";

        internal static readonly Meter Meter =
            new Meter(MeterName, typeof(GoogleMapsMetrics).Assembly.GetName().Version?.ToString());

        internal static readonly Counter<long> Requests = Meter.CreateCounter<long>(
            "gmaps.client.requests", unit: "{request}",
            description: "Number of Google Maps API calls attempted.");

        internal static readonly Histogram<double> RequestDuration = Meter.CreateHistogram<double>(
            "gmaps.client.request.duration", unit: "s",
            description: "Duration of Google Maps API calls.");

        internal static readonly Counter<long> RequestErrors = Meter.CreateCounter<long>(
            "gmaps.client.request.errors", unit: "{error}",
            description: "Number of Google Maps API calls that failed with an exception.");

        internal static void Record(
            string apiName,
            string method,
            int? statusCode,
            string? responseStatus,
            string? errorType,
            long startTimestamp)
        {
            if (!Requests.Enabled && !RequestDuration.Enabled && !RequestErrors.Enabled)
                return;

            // Stopwatch.GetElapsedTime is unavailable on netstandard2.0, so compute seconds directly.
            var elapsedSeconds = (Stopwatch.GetTimestamp() - startTimestamp) / (double)Stopwatch.Frequency;

            var requestTags = new TagList
            {
                { "gmaps.api", apiName },
                { "http.request.method", method },
            };

            Requests.Add(1, requestTags);

            var durationTags = requestTags;
            if (statusCode is not null)
                durationTags.Add("http.response.status_code", statusCode.Value);
            if (responseStatus is not null)
                durationTags.Add("gmaps.response_status", responseStatus);
            RequestDuration.Record(elapsedSeconds, durationTags);

            if (errorType is not null)
            {
                RequestErrors.Add(1,
                    new KeyValuePair<string, object?>("gmaps.api", apiName),
                    new KeyValuePair<string, object?>("error.type", errorType));
            }
        }
    }
}
