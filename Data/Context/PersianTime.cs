using System.Globalization;

namespace Data.Context;

public class PersianTime
{
    public static string GetNow()
    {
        // Create an instance of the PersianCalendar class
        PersianCalendar pc = new PersianCalendar();
        // Get the current date and time
        DateTime now = DateTime.Now;
        // Get the year, month, day, hour and minute of the current date and time in the Persian calendar
        int year = pc.GetYear(now);
        int month = pc.GetMonth(now);
        int day = pc.GetDayOfMonth(now);
        int hour = pc.GetHour(now);
        int minute = pc.GetMinute(now);
        int second = pc.GetSecond(now);

        return $"{year}/{month}/{day} - {hour}:{minute}:{second}";
    }
}