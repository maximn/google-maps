namespace GoogleMapsApi
{
    /// <summary>
    /// Configures a <see cref="GoogleMapsClient"/> instance.
    /// </summary>
    public sealed class GoogleMapsClientOptions
    {
        /// <summary>
        /// Default API key used for every request that does not set its own <c>ApiKey</c>.
        /// If a request explicitly sets its own <c>ApiKey</c>, that value is preserved.
        /// </summary>
        public string? ApiKey { get; set; }
    }
}
