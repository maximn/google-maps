using GoogleMapsApi.Test.Vcr;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace GoogleMapsApi.Test.Utils
{
    /// <summary>
    /// Marks a test or fixture as exercising a billable Google API (one whose usage exceeds the
    /// free quota and incurs charges, e.g. the Places, Aerial View and Solar APIs).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The gate only applies in a <em>live</em> <see cref="VcrMode"/> (record/auto/live), where the test
    /// actually reaches Google and can cost money: there it is skipped unless <c>RUN_BILLABLE_TESTS</c> is
    /// truthy (<c>1</c>, <c>true</c> or <c>yes</c>). In the default <c>replay</c> mode the response comes
    /// from a committed cassette — free and deterministic — so billable tests always run.
    /// </para>
    /// <para>
    /// The attribute also tags the test with the <c>Billable</c> category, so runners can
    /// additionally filter on it (e.g. <c>dotnet test --filter "TestCategory=Billable"</c>).
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class BillableTestAttribute : NUnitAttribute, IApplyToTest
    {
        internal const string EnableEnvironmentVariable = "RUN_BILLABLE_TESTS";
        internal const string Category = "Billable";

        public void ApplyToTest(NUnit.Framework.Internal.Test test)
        {
            test.Properties.Add(PropertyNames.Category, Category);

            if (test.RunState == RunState.NotRunnable || !VcrModes.IsLive || IsEnabled())
                return;

            test.RunState = RunState.Ignored;
            test.Properties.Set(PropertyNames.SkipReason,
                $"Billable API tests are skipped in live modes to avoid charges. Set {EnableEnvironmentVariable}=true to run them live, or use the default VCR_MODE=replay to run them from cassettes.");
        }

        private static bool IsEnabled()
        {
            var value = Environment.GetEnvironmentVariable(EnableEnvironmentVariable);

            return !string.IsNullOrWhiteSpace(value)
                && (value.Equals("1", StringComparison.OrdinalIgnoreCase)
                    || value.Equals("true", StringComparison.OrdinalIgnoreCase)
                    || value.Equals("yes", StringComparison.OrdinalIgnoreCase));
        }
    }
}
