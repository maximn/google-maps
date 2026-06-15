using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GoogleMapsApi.Test.Vcr
{
    /// <summary>
    /// A committed set of recorded interactions backing a single test, persisted as an indented JSON
    /// array at <see cref="FilePath"/>. Loading is tolerant of a missing file (empty cassette);
    /// <see cref="Save"/> creates the directory as needed.
    /// </summary>
    public sealed class Cassette
    {
        // Mirrors MapsAPIGenericEngine.secretQueryParameter (private there). Kept in sync intentionally so
        // cassettes never persist a real key/signature and record/replay match on the same redacted form.
        private static readonly Regex SecretQueryParameter =
            new(@"([?&](?:key|signature)=)[^&]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };

        private readonly List<RecordedInteraction> _interactions;
        private readonly Dictionary<string, int> _replayCursor = new();

        private Cassette(string filePath, List<RecordedInteraction> interactions)
        {
            FilePath = filePath;
            _interactions = interactions;
        }

        public string FilePath { get; }

        public IReadOnlyList<RecordedInteraction> Interactions => _interactions;

        internal static Cassette CreateEmpty(string filePath) =>
            new(filePath, new List<RecordedInteraction>());

        public static Cassette LoadOrEmpty(string filePath)
        {
            if (!File.Exists(filePath))
                return CreateEmpty(filePath);

            var json = File.ReadAllText(filePath);
            var interactions = JsonSerializer.Deserialize<List<RecordedInteraction>>(json)
                               ?? new List<RecordedInteraction>();
            return new Cassette(filePath, interactions);
        }

        public void Append(RecordedInteraction interaction) => _interactions.Add(interaction);

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(_interactions, WriteOptions));
        }

        /// <summary>Redacts <c>key=</c>/<c>signature=</c> query parameters from a URL.</summary>
        public static string Redact(string url) => SecretQueryParameter.Replace(url, "$1REDACTED");

        /// <summary>
        /// Canonicalizes a request body so insignificant formatting doesn't break matching: valid JSON is
        /// reparsed and reserialized, anything else is trimmed. <c>null</c> passes through (GET requests).
        /// </summary>
        public static string? NormalizeBody(string? body)
        {
            if (string.IsNullOrWhiteSpace(body))
                return null;

            try
            {
                using var doc = JsonDocument.Parse(body!);
                return JsonSerializer.Serialize(doc.RootElement);
            }
            catch (JsonException)
            {
                return body!.Trim();
            }
        }

        /// <summary>
        /// Returns the next recorded interaction matching the request, advancing a per-key cursor so a test
        /// that issues the same request more than once replays the recordings in order. <c>null</c> on miss.
        /// </summary>
        public RecordedInteraction? FindMatch(string method, string redactedUrl, string? normalizedBody)
        {
            var key = $"{method} {redactedUrl}\n{normalizedBody}";
            var seen = _replayCursor.TryGetValue(key, out var skip) ? skip : 0;

            var occurrence = 0;
            foreach (var interaction in _interactions)
            {
                if (interaction.MatchKey != key)
                    continue;

                if (occurrence++ < seen)
                    continue;

                _replayCursor[key] = seen + 1;
                return interaction;
            }

            return null;
        }
    }
}
