using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace GoogleMapsApi.Test.Utils
{
    /// <summary>
    /// Marks a test or fixture as exercising a billable Google API (one whose usage exceeds the
    /// free quota and incurs charges, e.g. the Places API). These tests are skipped by default to
    /// avoid running up the bill. Set the <c>RUN_BILLABLE_TESTS</c> environment variable to a
    /// truthy value (<c>1</c>, <c>true</c> or <c>yes</c>) to opt in and run them.
    /// </summary>
    /// <remarks>
    /// The attribute also tags the test with the <c>Billable</c> category, so runners can
    /// additionally filter on it (e.g. <c>dotnet test --filter "TestCategory=Billable"</c>).
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class BillableTestAttribute : NUnitAttribute, IApplyToTest
    {
        internal const string EnableEnvironmentVariable = "RUN_BILLABLE_TESTS";
        internal const string Category = "Billable";

        public void ApplyToTest(NUnit.Framework.Internal.Test test)
        {
            test.Properties.Add(PropertyNames.Category, Category);

            if (test.RunState == RunState.NotRunnable || IsEnabled())
                return;

            test.RunState = RunState.Ignored;
            test.Properties.Set(PropertyNames.SkipReason,
                $"Billable API tests are skipped by default to avoid charges. Set {EnableEnvironmentVariable}=true to run them.");
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
