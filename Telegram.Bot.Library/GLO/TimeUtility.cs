using System;

namespace Telegram.Bot.Library.GLO
{
    public static class TimeUtility
    {
        private static readonly DateTime s_intiailTime = new DateTime(1970, 1, 1);

        public static long DateToMilliseconds(DateTime dateTime)
        {
            return (long)(dateTime - s_intiailTime).TotalMilliseconds;
        }
        
    }
}
