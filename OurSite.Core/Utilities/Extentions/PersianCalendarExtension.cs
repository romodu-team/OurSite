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
                var dateArray = PersianDate.Split("/");

                var date = pc.ToDateTime(int.Parse(dateArray[0]),int.Parse(dateArray[1]),int.Parse(dateArray[2]),0,0,0,0);
                return date;

        }
    }
}

