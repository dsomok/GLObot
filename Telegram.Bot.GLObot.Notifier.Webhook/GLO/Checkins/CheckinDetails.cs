using System;

namespace Telegram.Bot.GLObot.Notifier.Webhook.GLO.Checkins
{
    class CheckinDetails : CheckinBase
    {
        public CheckinDetails(string area, TimeSpan secondsAgo, CheckinDirection direction, string timestamp) : base(area, direction, timestamp)
        {
            SecondsAgo = secondsAgo;
        }

        public TimeSpan SecondsAgo { get; private set; }

    }
}