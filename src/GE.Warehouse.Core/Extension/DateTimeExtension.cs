using System;
using System.Diagnostics;

namespace GE.Warehouse.Core.Extension
{
    public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        [DebuggerStepThrough]
        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        [DebuggerStepThrough]
        public static string ToVnTime(this DateTime? time)
        {
            return time.Value.Hour + ":" + time.Value.Minute;
        }

        [DebuggerStepThrough]
        public static string ToVnTime(this DateTime time)
        {
            return time.Hour + ":" + time.Minute;
        }

        [DebuggerStepThrough]
        public static string ToVnDate(this DateTime time)
        {
            return time.Day + "/" + time.Month + "/" + time.Year;
        }

        public static long GetJavascriptTimestamp(this DateTime dateTime)
        {
            var span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
            var time = dateTime.Subtract(span);
            return time.Ticks / 10000;
            //return (dateTime.ToUniversalTime().Ticks - 621355968000000000)/10000000;
        }

    }
}