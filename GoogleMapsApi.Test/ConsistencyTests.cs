using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Cross-cutting invariants that keep the request and entity surface uniform (issue #298).
    /// These are reflection-driven on purpose: a new API that forgets the convention fails here
    /// rather than quietly re-introducing the inconsistency.
    /// </summary>
    [TestFixture]
    public class ConsistencyTests
    {
        private static readonly Assembly LibraryAssembly = typeof(MapsBaseRequest).Assembly;

        private static IEnumerable<Type> ConcreteRequestTypes =>
            LibraryAssembly.GetTypes()
                .Where(t => typeof(MapsBaseRequest).IsAssignableFrom(t) && !t.IsAbstract);

        private static IEnumerable<Type> PublicEnums =>
            LibraryAssembly.GetTypes().Where(t => t.IsEnum && t.IsPublic);

        /// <summary>
        /// Every request must resolve to HTTPS. Requests with unmet required properties throw while
        /// building their URI; those are skipped, but the covered count is asserted so this test
        /// cannot silently decay into checking nothing.
        /// </summary>
        [Test]
        public void AllRequests_ProduceHttpsUris()
        {
            var covered = new List<string>();
            var violations = new List<string>();

            foreach (var type in ConcreteRequestTypes)
            {
                if (type.GetConstructor(Type.EmptyTypes) == null)
                    continue;

                var request = (MapsBaseRequest)Activator.CreateInstance(type)!;
                request.ApiKey = "test-api-key";

                Uri uri;
                try
                {
                    uri = request.GetUri();
                }
                catch (Exception)
                {
                    // Required properties not set for this request type - not what we're testing.
                    continue;
                }

                covered.Add(type.Name);
                if (uri.Scheme != Uri.UriSchemeHttps)
                    violations.Add($"{type.Name} -> {uri.Scheme}");
            }

            Assert.That(violations, Is.Empty, "Requests must always use HTTPS");
            Assert.That(covered, Has.Count.GreaterThanOrEqualTo(5),
                "Too few request types exercised - the reflection filter has decayed");
        }

        /// <summary>
        /// No request type may declare its own SSL knob; HTTPS is enforced centrally in
        /// <see cref="MapsBaseRequest"/>.
        /// </summary>
        [Test]
        public void NoRequestType_DeclaresItsOwnIsSslMember()
        {
            var offenders = ConcreteRequestTypes
                // Compatibility shim for a shipped override; remove with IsSSL in 3.0.
                .Where(t => t != typeof(TimeZoneRequest))
                .Where(t => t.GetProperty("IsSSL", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly) != null)
                .Select(t => t.Name)
                .ToList();

            Assert.That(offenders, Is.Empty,
                "SSL enforcement lives in MapsBaseRequest; request types must not redeclare it");
        }

        /// <summary>
        /// Regression for the Static Maps API-key leak: the generated URL embedded the caller's key
        /// in a plain-HTTP URL because <c>IsSSL</c> defaulted to false.
        /// </summary>
        [Test]
        public void StaticMapUrl_IsHttps_AndNeverLeaksApiKeyOverPlainHttp()
        {
            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(400, 400))
            {
                ApiKey = "secret-api-key"
            };

            var url = new StaticMapsEngine().GenerateStaticMapURL(request);

            Assert.That(url, Does.StartWith("https://"));
            Assert.That(url, Does.Not.StartWith("http://"));
            Assert.That(url, Does.Contain("secret-api-key"), "sanity: the key is in fact part of the URL");
        }

        /// <summary>
        /// Locks the documented enum convention (.agents/serialization.md): carry
        /// <see cref="EnumMemberAttribute"/> only when Google's wire value actually differs from the
        /// C# member name. The converter falls back to the member name, so a valueless attribute or
        /// one whose value equals the name is pure noise.
        /// </summary>
        [Test]
        public void EnumMemberAttributes_AreOnlyPresentWhenTheWireValueDiffers()
        {
            var redundant = new List<string>();

            foreach (var enumType in PublicEnums)
            {
                foreach (var member in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var attribute = member.GetCustomAttribute<EnumMemberAttribute>();
                    if (attribute == null)
                        continue;

                    if (attribute.Value == null)
                        redundant.Add($"{enumType.FullName}.{member.Name} (no Value)");
                    else if (string.Equals(attribute.Value, member.Name, StringComparison.Ordinal))
                        redundant.Add($"{enumType.FullName}.{member.Name} (Value == name)");
                }
            }

            Assert.That(redundant, Is.Empty,
                "Redundant [EnumMember] attributes - the converter already falls back to the member name");
        }
    }
}
