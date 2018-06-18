using System;

namespace Telegram.Bot.Library.GLO.Checkins
{
    public class CheckinDetails : CheckinBase
    {
        public CheckinDetails(string area, TimeSpan secondsAgo, CheckinDirection direction, string timestamp) : base(area, direction, timestamp)
        {
            SecondsAgo = secondsAgo;
        }

        public TimeSpan SecondsAgo { get; private set; }

    }
}