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
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(epoch), expected);
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(epoch.ToLocalTime()), expected);
        }

        [Test]
        public void DateTimeToUnixTimestamp_DST_ExpectedResult()
        {
            var dst = new DateTime(2016, 4, 4, 10, 0, 0, DateTimeKind.Utc);
            const int expected = 1459764000;
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(dst), expected);
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(dst.ToLocalTime()), expected);
        }

        [Test]
        public void DateTimeToUnixTimestamp_NonDST_ExpectedResult()
        {
            var nonDst = new DateTime(2016, 3, 1, 11, 0, 0, DateTimeKind.Utc);
            const int expected = 1456830000;
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(nonDst), expected);
            Assert.AreEqual(UnixTimeConverter.DateTimeToUnixTimestamp(nonDst.ToLocalTime()), expected);
        }
    }
}