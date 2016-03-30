using System;
using System.Reflection;
using NUnit.Framework;
using GoogleMapsApi.Engine;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class UnixTimeConverterTest
    {
        /// <summary>
        /// This test verifies that the DateTimeToUnixTimestamp prdouces the expected Unix timestamps
        /// </summary>
        [Test]
        public void DateTimeToUnixTimestamp_Test()
        {
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(new DateTime(2016, 4, 4, 12, 0, 0, DateTimeKind.Local)), 1459764000);
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(new DateTime(2016, 4, 4, 10, 0, 0, DateTimeKind.Utc)), 1459764000);
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(new DateTime(2016, 3, 1, 12, 0, 0, DateTimeKind.Local)), 1456830000);
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(new DateTime(2016, 3, 1, 11, 0, 0, DateTimeKind.Utc)), 1456830000);
        }
    }
}
