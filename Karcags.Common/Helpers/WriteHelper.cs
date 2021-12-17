using System;
using System.Collections.Generic;

namespace Karcags.Common.Helpers
{
    /// <summary>
    /// Display helper
    /// </summary>
    public static class WriteHelper
    {
        /// <summary>
        /// Add leader zero
        /// </summary>
        /// <param name="number">Base number</param>
        /// <param name="width">Destination width</param>
        /// <returns>String number with given length and filled with zero</returns>
        public static string LeaderZero(int number, int width)
        {
            return number.ToString().PadLeft(width, '0');
        }

        /// <summary>
        /// Write nullable field
        /// </summary>
        /// <param name="fieldValue">Field object</param>
        /// <returns>Not null object or N/A string</returns>
        public static string WriteNullableField(object fieldValue)
        {
            return fieldValue == null ? "N/A" : fieldValue.ToString();
        }

        /// <summary>
        /// Write empty field
        /// </summary>
        /// <param name="val">Text value</param>
        /// <returns>Not empty text or N/A string</returns>
        public static string WriteEmptyableField(string val)
        {
            return string.IsNullOrEmpty(val) ? "N/A" : val;
        }

        /// <summary>
        /// Write Ft (Forint)
        /// </summary>
        /// <param name="fieldValue">Number field</param>
        /// <returns>Appended text with Ft suffix</returns>
        public static string WriteForint(decimal? fieldValue)
        {
            return fieldValue == null ? "-" : $"{fieldValue} Ft";
        }

        /// <summary>
        /// Write list as string
        /// </summary>
        /// <param name="list">String list for append</param>
        /// <param name="separator">Separator</param>
        /// <returns>Appended text separated by the separator value</returns>
        public static string WriteList(List<string> list, string separator)
        {
            // Empty check
            if (list == null || list.Count == 0)
            {
                return "N/A";
            }

            return string.Join($"{separator} ", list);
        }

        public static string HourInterval(DateTime date, int count)
        {
            return $"{date.ToLocalTime().Hour}h - {date.ToLocalTime().Hour + count}h";
        }
    }
}