using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.Vcr
{
    /// <summary>
    /// A <see cref="DelegatingHandler"/> that records live HTTP exchanges to a cassette and/or replays
    /// them, per the active <see cref="VcrMode"/>. One handler is bound to one cassette file (one test).
    /// </summary>
    /// <remarks>
    /// In replay the handler yields and honors the cancellation token before returning, so the engine's
    /// cancellation/timeout plumbing still behaves correctly when there is no real network round-trip.
    /// </remarks>
    public sealed class VcrDelegatingHandler : DelegatingHandler
    {
        private readonly VcrMode _mode;
        private readonly Cassette _cassette;
        private readonly object _gate = new();

        public VcrDelegatingHandler(VcrMode mode, string cassetteFilePath, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            _mode = mode;
            _cassette = mode == VcrMode.Record
                ? Cassette.CreateEmpty(cassetteFilePath)
                : Cassette.LoadOrEmpty(cassetteFilePath);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_mode == VcrMode.Live)
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var method = request.Method.Method;
            var redactedUrl = Cassette.Redact(request.RequestUri!.AbsoluteUri);
            var requestBody = request.Content == null
                ? null
                : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var normalizedBody = Cassette.NormalizeBody(requestBody);

            if (_mode == VcrMode.Replay || _mode == VcrMode.Auto)
            {
                // Give callers a chance to cancel immediately after starting the request, including
                // cancellation-only tests that intentionally have no cassette.
                await Task.Yield();
                cancellationToken.ThrowIfCancellationRequested();

                var match = _cassette.FindMatch(method, redactedUrl, normalizedBody);
                if (match != null)
                    return BuildResponse(match);

                if (_mode == VcrMode.Replay)
                    throw new InvalidOperationException(
                        $"No recorded response for {method} {redactedUrl} in cassette '{_cassette.FilePath}'. " +
                        $"Re-record it with: VCR_MODE=record GOOGLE_API_KEY=<key> RUN_BILLABLE_TESTS=true dotnet test");
            }

            // Record (or Auto miss): hit the network, persist the exchange, then return a buffered copy.
            using var live = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var bytes = await live.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            var recorded = new RecordedInteraction
            {
                Method = method,
                Url = redactedUrl,
                RequestBody = normalizedBody,
                StatusCode = (int)live.StatusCode,
                ContentType = live.Content.Headers.ContentType?.ToString(),
                BodyBase64 = Convert.ToBase64String(bytes),
            };

            lock (_gate)
            {
                _cassette.Append(recorded);
                _cassette.Save();
            }

            return BuildResponse(recorded);
        }

        private static HttpResponseMessage BuildResponse(RecordedInteraction interaction)
        {
            var content = new ByteArrayContent(Convert.FromBase64String(interaction.BodyBase64));
            if (!string.IsNullOrEmpty(interaction.ContentType))
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(interaction.ContentType);

            return new HttpResponseMessage((System.Net.HttpStatusCode)interaction.StatusCode)
            {
                Content = content,
            };
        }
    }
}
