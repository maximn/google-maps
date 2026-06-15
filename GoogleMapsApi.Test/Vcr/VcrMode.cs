using System;

namespace GoogleMapsApi.Test.Vcr
{
    /// <summary>
    /// Controls how the integration test suite talks to Google, selected by the
    /// <c>VCR_MODE</c> environment variable (default <see cref="Replay"/>).
    /// </summary>
    public enum VcrMode
    {
        /// <summary>Serve responses from committed cassettes; fail loudly if none matches. No key, no network, no charge.</summary>
        Replay,

        /// <summary>Call live Google and (over)write the cassette on disk. Needs a real key; incurs charges.</summary>
        Record,

        /// <summary>Replay when a matching cassette exists, otherwise record the missing interaction live.</summary>
        Auto,

        /// <summary>Pass straight through to Google, never reading or writing cassettes (the pre-VCR behavior).</summary>
        Live,
    }

    /// <summary>
    /// Resolves and exposes the active <see cref="VcrMode"/> for the current test run.
    /// </summary>
    public static class VcrModes
    {
        internal const string EnvironmentVariable = "VCR_MODE";

        /// <summary>The mode for this run, parsed once from <c>VCR_MODE</c>.</summary>
        public static VcrMode Current { get; } = Parse(Environment.GetEnvironmentVariable(EnvironmentVariable));

        /// <summary>
        /// True for any mode that may reach Google (and therefore may cost money). Used by
        /// <c>BillableTestAttribute</c> to decide whether the billable gate applies.
        /// </summary>
        public static bool IsLive => Current != VcrMode.Replay;

        internal static VcrMode Parse(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return VcrMode.Replay;

            return value!.Trim().ToLowerInvariant() switch
            {
                "record" => VcrMode.Record,
                "auto" => VcrMode.Auto,
                "live" => VcrMode.Live,
                "replay" => VcrMode.Replay,
                _ => throw new InvalidOperationException(
                    $"Unknown {EnvironmentVariable} value '{value}'. Expected one of: replay, record, auto, live."),
            };
        }
    }
}
