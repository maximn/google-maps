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
            Assert.That(location.ToString(), Is.EqualTo("52.123123,0.0"));
        }
    }
}
