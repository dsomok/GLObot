using System;

namespace Telegram.Bot.GLObot.Notifier.Webhook.GLO.Checkins
{
    public class CheckinStats
    {
        public CheckinStats(TimeSpan workingTimeToday, byte teleportsCount, TimeSpan lostTeleportTime, DateTime? firstCheckinToday)
        {
            WorkingTimeToday = workingTimeToday;
            TeleportsCount = teleportsCount;
            LostTeleportTime = lostTeleportTime;
            FirstCheckinToday = firstCheckinToday;
        }
        public TimeSpan WorkingTimeToday { get; private set; }
        public byte TeleportsCount { get; private set; }
        public DateTime? FirstCheckinToday { get; private set; }
        public TimeSpan LostTeleportTime { get; }

        public TimeSpan TimeWithTeleports => WorkingTimeToday + LostTeleportTime;
    }
}
