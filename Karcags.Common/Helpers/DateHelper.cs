using System;

namespace Karcags.Common.Helpers
{
    /// <summary>
    /// Date helper
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Date to string.
        /// Format [year]-[month:number]-[day:number] [hour]:[mint]:[sec].
        /// </summary>
        /// <param name="date">Input date</param>
        /// <returns>Formatted text</returns>
        public static string DateToString(DateTime date)
        {
            return
                $"{date.Year}-{WriteHelper.LeaderZero(date.Month, 2)}-{WriteHelper.LeaderZero(date.Day, 2)} {WriteHelper.LeaderZero(date.Hour, 2)}:{WriteHelper.LeaderZero(date.Minute, 2)}:{WriteHelper.LeaderZero(date.Second, 2)}";
        }

        /// <summary>
        /// Nullable date to string.
        /// Format [year]-[month:number]-[day:number] [hour]:[mint]:[sec].
        /// </summary>
        /// <param name="date">Input date</param>
        /// <returns>Formatted text or N/A if the date is null</returns>
        public static string DateToString(DateTime? date)
        {
            return date == null ? "N/A" : DateToString((DateTime)date);
        }

        /// <summary>
        /// Date to month string.
        /// Format [year] [month:string].
        /// </summary>
        /// <param name="date">Input date</param>
        /// <returns>Formatted text</returns>
        public static string DateToMonthString(DateTime date)
        {
            return $"{date:yyyy MMMM}";
        }

        /// <summary>
        /// Date to week string.
        /// Interval of a week with start and end date.
        /// </summary>
        /// <param name="date">Start date</param>
        /// <returns>Week interval</returns>
        public static string DateToWeekString(DateTime date)
        {
            return $"{DateToDayString(date)} - {DateToDayString(date.AddDays(6))}";
        }

        /// <summary>
        /// Date to day string.
        /// Format [year] [month:string] [day:number]
        /// </summary>
        /// <param name="date">Input date</param>
        /// <returns>Formatted text</returns>
        public static string DateToDayString(DateTime date)
        {
            return $"{date:yyyy MMMM dd}";
        }

        /// <summary>
        /// Date to number day string.
        /// Format [year]-[month:number]-[day:number]
        /// </summary>
        /// <param name="date">Input date</param>
        /// <returns>Formatted text</returns>
        public static string DateToNumberDayString(DateTime date)
        {
            return $"{date:yyyy-MM-dd}";
        }

        /// <summary>
        /// To day
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns></returns>
        public static DateTime ToDay(DateTime date)
        {
            return new(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// CompareDates by Year, Month, Day and Hour
        /// </summary>
        /// <param name="date1">Date a</param>
        /// <param name="date2">Date b</param>
        /// <returns>True if the two date is equal to each other</returns>
        public static bool CompareDates(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day &&
                   date1.Hour == date2.Hour;
        }

        /// <summary>
        /// Convert Date to file name compatible format.
        /// [Year]-[Month]-[Day]-[Hour]-[Min]-[Sec]
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>Formatted text</returns>
        public static string ToFileName(DateTime date)
        {
            return $"{date:yyyy-MM-dd-HH-mm-ss}";
        }
    }
}