using System;
using Karcags.Common.Helpers;
using NUnit.Framework;

namespace Karcags.Common.Tests.Helpers
{
    [TestFixture]
    public class DateHelperTest
    {
        [Test]
        public void DateToString_WithNullDateTime_ReturnNA()
        {
            var result = DateHelper.DateToString(null);

            Assert.That(result, Is.EqualTo("N/A"));
        }

        [Test]
        public void DateToString_WithNotNullFullDateTime_ReturnCorrectDateString()
        {
            var result = DateHelper.DateToString(new DateTime(2012, 4, 3, 12, 14, 32));

            Assert.That(result, Is.EqualTo("2012-04-03 12:14:32"));
        }

        [Test]
        public void DateToString_WithNotNullNotFullDateTime_ReturnCorrectDateString()
        {
            var result = DateHelper.DateToString(new DateTime(2012, 4, 3));

            Assert.That(result, Is.EqualTo("2012-04-03 00:00:00"));
        }

        [Test]
        public void DateToMonthString_WithDateTime_ReturnCorrectFullMonthNameWithYear()
        {
            var result = DateHelper.DateToMonthString(new DateTime(2012, 4, 3));

            Assert.That(result, Is.EqualTo("2012 April"));
        }

        [Test]
        public void DateToWeekString_WithDateTime_ReturnAWeekIntervalInString()
        {
            var result = DateHelper.DateToWeekString(new DateTime(2012, 4, 3));

            Assert.That(result, Is.EqualTo("2012 April 03 - 2012 April 09"));
        }

        [Test]
        public void DateToDayString_WithDateTime_ReturnAStringWithYearFullMonthNameAndDay()
        {
            var result = DateHelper.DateToDayString(new DateTime(2014, 7, 23));

            Assert.That(result, Is.EqualTo("2014 July 23"));
        }

        [Test]
        public void DateToNumberDayString_WithDateTime_ReturnAStringWithYearMonthAndDay()
        {
            var result = DateHelper.DateToNumberDayString(new DateTime(2014, 7, 23));

            Assert.That(result, Is.EqualTo("2014-07-23"));
        }

        [Test]
        public void ToDay_WithDateTimeWithHourMinAndSec_ReturnWithDateTimeWith0HourMinAndSec()
        {
            var result = DateHelper.ToDay(new DateTime(2032, 11, 12, 12, 23, 43));

            Assert.That(result, Is.EqualTo(new DateTime(2032, 11, 12, 0, 0, 0)));
        }
        
        [Test]
        public void CompareDates_FirstDateInEarlierYear_ReturnFalse()
        {
            var result = DateHelper.CompareDates(new DateTime(2032, 11, 12, 12, 0, 43), new DateTime(2034, 11, 11, 12, 0, 0));

            Assert.That(result, Is.False);
        }
        
        [Test]
        public void CompareDates_FirstDateInEarlierMonth_ReturnFalse()
        {
            var result = DateHelper.CompareDates(new DateTime(2032, 8, 12, 12, 0, 43), new DateTime(2032, 11, 11, 12, 0, 0));

            Assert.That(result, Is.False);
        }
        
        [Test]
        public void CompareDates_FirstDateInEarlierDay_ReturnFalse()
        {
            var result = DateHelper.CompareDates(new DateTime(2032, 8, 7, 12, 0, 43), new DateTime(2032, 8, 11, 12, 0, 0));

            Assert.That(result, Is.False);
        }
        
        [Test]
        public void CompareDates_TwoDayIsInTheSameYearMonthDayHourMinAndSec_ReturnTrue()
        {
            var result = DateHelper.CompareDates(new DateTime(2032, 8, 7, 12, 10, 10), new DateTime(2032, 8, 7, 12, 10, 10));

            Assert.That(result, Is.True);
        }
        
        [Test]
        public void CompareDates_TwoDayIsInTheSameYearMonthDayAndHourButNotMinAndSec_ReturnTrue()
        {
            var result = DateHelper.CompareDates(new DateTime(2032, 8, 7, 12, 13, 7), new DateTime(2032, 8, 7, 12, 10, 10));

            Assert.That(result, Is.True);
        }
        
        [Test]
        public void CompareDates_SecondDateInEarlierYear_ReturnFalse()
        {
            var result = DateHelper.CompareDates(new DateTime(2040, 11, 12, 12, 0, 43), new DateTime(2034, 11, 11, 7, 0, 0));

            Assert.That(result, Is.False);
        }
        
        [Test]
        public void CompareDates_SecondDateInEarlierMonth_ReturnFalse()
        {
            var result = DateHelper.CompareDates(new DateTime(2032, 8, 12, 12, 0, 43), new DateTime(2032, 5, 11, 5, 0, 0));

            Assert.That(result, Is.False);
        }
        
        [Test]
        public void CompareDates_SecondDateInEarlierDay_ReturnFalse()
        {
            var result = DateHelper.CompareDates(new DateTime(2032, 8, 7, 12, 0, 43), new DateTime(2032, 8, 2, 11, 0, 0));

            Assert.That(result, Is.False);
        }

        [Test]
        public void ToFileName_WithDateTime_ReturnFileNameCompatibleString()
        {
            var result = DateHelper.ToFileName(new DateTime(2012, 2, 3, 4, 5, 5));
            Assert.That(result, Is.EqualTo("2012-02-03-04-05-05"));
        }
    }
}