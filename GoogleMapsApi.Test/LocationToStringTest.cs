using GoogleMapsApi.Entities.Common;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class LocationToStringTest
    {
        [Test]
        public void WhenNearZeroLongitude_ExpectCorrectToString()
        {
            // Longitude of 0.000009 is converted to 9E-06 using Invariant ToString, but we need 0.000009
            var location = new Location(57.231d, 0.000009d);
            Assert.That(location.ToString(), Is.EqualTo("57.231,0.000009"));
        }

        [Test]
        public void WhenZeroLongitude_ExpectCorrectToString()
        {
            // Longitude of 0.000009 is converted to 9E-06 using Invariant ToString, but we need 0.000009
            var location = new Location(52.123123d, 0.0d);
            Assert.That(location.ToString(), Is.EqualTo("52.123123,0"));
        }

        [Test]
        public void WhenNegativeScientificNotation_ExpectDecimalFormat()
        {
            // Issue #160: -1e-05 should be rendered as decimal, not scientific notation
            var location = new Location(-1e-05, 0.0d);
            Assert.That(location.ToString(), Is.EqualTo("-0.00001,0"));
        }

        [Test]
        public void WhenVerySmallNumbers_ExpectDecimalNotScientific()
        {
            // Test various small numbers to ensure no scientific notation
            var location1 = new Location(1e-10, 0.0d);
            var location2 = new Location(-5e-15, 0.0d);
            var location3 = new Location(0.0d, 3.14e-8);
            
            // Should not contain 'e' or 'E'
            Assert.That(location1.ToString(), Does.Not.Contain("e"));
            Assert.That(location1.ToString(), Does.Not.Contain("E"));
            Assert.That(location2.ToString(), Does.Not.Contain("e"));
            Assert.That(location2.ToString(), Does.Not.Contain("E"));
            Assert.That(location3.ToString(), Does.Not.Contain("e"));
            Assert.That(location3.ToString(), Does.Not.Contain("E"));
        }

        [Test]
        public void WhenIntegerValues_ExpectCorrectToString()
        {
            // Regression test for issue where 10.0 was rendered as "1,0" instead of "10,0"
            var location1 = new Location(10.0d, 0.0d);
            var location2 = new Location(100.0d, 50.0d);
            var location3 = new Location(1.0d, 20.0d);
            
            Assert.That(location1.ToString(), Is.EqualTo("10,0"));
            Assert.That(location2.ToString(), Is.EqualTo("100,50"));
            Assert.That(location3.ToString(), Is.EqualTo("1,20"));
        }
    }
}
