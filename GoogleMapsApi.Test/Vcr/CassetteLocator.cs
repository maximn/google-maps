using System;
using System.IO;
using System.Linq;

namespace GoogleMapsApi.Test.Vcr
{
    /// <summary>
    /// Maps a test to its cassette file under the test project's source <c>Cassettes/</c> directory, so
    /// recordings are written back into the repo (not the throwaway <c>bin/</c> output) and committed.
    /// </summary>
    public static class CassetteLocator
    {
        private const string ProjectFileName = "GoogleMapsApi.Test.csproj";
        private const string CassettesDirName = "Cassettes";

        private static readonly Lazy<string> CassettesRoot = new(ResolveCassettesRoot);

        /// <summary>
        /// Cassette path for a test: <c>Cassettes/&lt;FixtureName&gt;/&lt;TestName&gt;.json</c>. The fixture
        /// is the simple class name; both segments are sanitized to safe file names.
        /// </summary>
        public static string ForTest(string fixtureClassName, string testName)
        {
            var fixture = Sanitize(SimpleName(fixtureClassName));
            var name = Sanitize(testName);
            return Path.Combine(CassettesRoot.Value, fixture, name + ".json");
        }

        private static string SimpleName(string fullClassName)
        {
            var lastDot = fullClassName.LastIndexOf('.');
            return lastDot >= 0 ? fullClassName.Substring(lastDot + 1) : fullClassName;
        }

        private static string Sanitize(string value)
        {
            var invalid = Path.GetInvalidFileNameChars();
            return new string(value.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
        }

        private static string ResolveCassettesRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, ProjectFileName)))
                dir = dir.Parent;

            if (dir == null)
                throw new InvalidOperationException(
                    $"Could not locate '{ProjectFileName}' above '{AppContext.BaseDirectory}' to resolve the cassette directory.");

            return Path.Combine(dir.FullName, CassettesDirName);
        }
    }
}
