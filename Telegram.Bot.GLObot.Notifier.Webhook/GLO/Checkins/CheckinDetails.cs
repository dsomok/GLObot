using System;

namespace Telegram.Bot.GLObot.Notifier.Webhook.GLO.Checkins
{
    enum CheckinDirection
    {
        In,
        Out
    }

    class CheckinDetails
    {
        public CheckinDetails(string area, TimeSpan secondsAgo, CheckinDirection direction, string timestamp)
        {
            Area = area;
            SecondsAgo = secondsAgo;
            Direction = direction;
            Timestamp = timestamp;
        }

        public string Area { get; private set; }

        public TimeSpan SecondsAgo { get; private set; }

        public CheckinDirection Direction { get; private set; }

        public string DirectionName => Enum.GetName(typeof(CheckinDirection), this.Direction);

        public string Timestamp { get; private set; }
    }
}
