using System;
using System.Globalization;

namespace OurSite.Core.Utilities.Extentions
{
    public static class PersianCalendarExtension
    {
        public static string PersianDate(this DateTime datetime)
        {

                PersianCalendar pc = new PersianCalendar();
                string date = $"{pc.GetYear((DateTime)datetime)}/{pc.GetMonth((DateTime)datetime)}/{pc.GetDayOfMonth((DateTime)datetime)}";
                return date;

                
        }

        public static DateTime AdDate(this string PersianDate)
        {

                PersianCalendar pc = new PersianCalendar();
                var date = pc.ToDateTime(1401, 01, 01, 0 , 0 , 0 , 0);
                return date;

        }
    }
}

