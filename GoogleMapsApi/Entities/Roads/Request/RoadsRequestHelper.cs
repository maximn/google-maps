using System;
using System.Collections.Generic;
using System.Linq;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Roads.Request
{
    /// <summary>
    /// Shared validation and URL-building helpers for the Roads API requests. The Roads endpoints
    /// live on a different host than the rest of the Maps Web Services, so they build their URIs by
    /// hand rather than going through <see cref="QueryStringParametersList"/>.
    /// </summary>
    internal static class RoadsRequestHelper
    {
        /// <summary>
        /// Validates that an API key is present and SSL is enabled (required for every Roads endpoint).
        /// </summary>
        public static void ValidateApiKeyAndSsl(string? apiKey, bool isSsl)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("ApiKey is required for the Roads API.", nameof(apiKey));
            if (!isSsl)
                throw new ArgumentException("Roads API requires SSL [IsSSL = true].");
        }

        /// <summary>
        /// Validates a sequence of points and renders it as a pipe-separated, URL-escaped
        /// <c>lat,lng|lat,lng</c> parameter value.
        /// </summary>
        /// <param name="points">The points to serialize.</param>
        /// <param name="maxPoints">Maximum number of points the endpoint accepts.</param>
        /// <param name="parameterName">Name of the owning property, used in exception messages.</param>
        public static string BuildPointsParameter(IEnumerable<Location>? points, int maxPoints, string parameterName)
        {
            if (points == null)
                throw new ArgumentException($"{parameterName} is required.", parameterName);

            var list = points.ToList();
            if (list.Count == 0)
                throw new ArgumentException($"{parameterName} must contain at least one point.", parameterName);
            if (list.Count > maxPoints)
                throw new ArgumentException($"{parameterName} may contain at most {maxPoints} points.", parameterName);

            return string.Join("|", list.Select(p => Uri.EscapeDataString(p.LocationString)));
        }
    }
}
