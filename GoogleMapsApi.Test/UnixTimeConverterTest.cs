using System;
using NUnit.Framework;
using GoogleMapsApi.Engine;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class UnixTimeConverterTest
    {
        [Test]
        public void DateTimeToUnixTimestamp_Zero_ExpectedResult()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            const int expected = 0;
            
            // Updated Assertions
            Assert.That(UnixTimeConverter.DateTimeToUnixTimestamp(epoch), Is.EqualTo(expected));
            Assert.That(UnixTimeConverter.DateTimeToUnixTimestamp(epoch.ToLocalTime()), Is.EqualTo(expected));
        }

        [Test]
        public void DateTimeToUnixTimestamp_DST_ExpectedResult()
        {
            var dst = new DateTime(2016, 4, 4, 10, 0, 0, DateTimeKind.Utc);
            const int expected = 1459764000;
            
            // Updated Assertions
            Assert.That(UnixTimeConverter.DateTimeToUnixTimestamp(dst), Is.EqualTo(expected));
            Assert.That(UnixTimeConverter.DateTimeToUnixTimestamp(dst.ToLocalTime()), Is.EqualTo(expected));
        }

        [Test]
        public void DateTimeToUnixTimestamp_NonDST_ExpectedResult()
        {
            var nonDst = new DateTime(2016, 3, 1, 11, 0, 0, DateTimeKind.Utc);
            const int expected = 1456830000;
            
            // Updated Assertions
            Assert.That(UnixTimeConverter.DateTimeToUnixTimestamp(nonDst), Is.EqualTo(expected));
            Assert.That(UnixTimeConverter.DateTimeToUnixTimestamp(nonDst.ToLocalTime()), Is.EqualTo(expected));
        }
    }
}