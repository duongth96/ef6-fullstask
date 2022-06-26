using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.Warehouse.Services
{
    public static class DateTimeExtensions
    {
        enum Phase { Years, Months, Days, Done }

        public static int CountSunday(this DateTime dateTime, int during)
        {
            var dates = new List<DateTime>();
            for (int i = 0; i < during; i++)
            {
                dates.Add(dateTime.AddDays(i));
            }
            return dates.Count(d => d.DayOfWeek == DayOfWeek.Sunday);
        }

        public static DateTime DateWorkAfter(this DateTime dateTime, int during)
        {
            var endDate = dateTime.AddDays(during - 1);
            var sundays = dateTime.CountSunday(during - 1);
            while (sundays != 0)
            {
                var newcount = endDate.CountSunday(sundays);
                endDate = endDate.AddDays(sundays);
                sundays = newcount;
            }
            if (endDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return endDate.AddDays(1);
            }
            return endDate;
        }

        public static int GetMonthsBetween(this DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                var sub = date1;
                date1 = date2;
                date2 = sub;
            }
            return ((date2.Year - date1.Year)*12) + date2.Month - date1.Month;
        }
    }

    


}