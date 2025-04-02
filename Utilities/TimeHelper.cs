using System;

namespace DotNetInterview.API.Utilities
{
    public static class TimeHelper
    {
        public static bool IsMondayAfternoonLondon()
        {
            var londonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var londonTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow, 
                londonTimeZone);
            
            return londonTime.DayOfWeek == DayOfWeek.Monday 
                   && londonTime.Hour >= 12 
                   && londonTime.Hour < 17;
        }
    }
}