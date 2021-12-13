using System;

namespace Services
{
    public static class TimeService
    {
        public static long GetCurrentUnixTime()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        public static DateTime GetDateFromUnitTime(long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTime);
        }
    }
}
