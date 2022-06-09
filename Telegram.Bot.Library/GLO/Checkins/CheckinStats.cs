using System;

namespace Telegram.Bot.Library.GLO.Checkins
{
    public class CheckinStats
    {
        public CheckinStats(TimeSpan workingTimeToday, byte teleportsCount, TimeSpan lostTeleportTime, DateTime? firstCheckinToday, CheckinDetails lastCheckin)
        {
            WorkingTimeToday = workingTimeToday;
            TeleportsCount = teleportsCount;
            LostTeleportTime = lostTeleportTime;
            FirstCheckinToday = firstCheckinToday;
            LastCheckin = lastCheckin;
        }
        public TimeSpan WorkingTimeToday { get; private set; }
        public byte TeleportsCount { get; private set; }
        public DateTime? FirstCheckinToday { get; private set; }
        public TimeSpan LostTeleportTime { get; }
        public CheckinDetails LastCheckin { get; private set; }

        public TimeSpan TimeWithTeleports => WorkingTimeToday + LostTeleportTime;

        public string FirstCheckinFormated => FirstCheckinToday.HasValue ? ((DateTime) FirstCheckinToday).ToString("HH:mm:ss") : "";

        public void UpdateLastCheckin(CheckinDetails lastCheckin)
        {
            this.LastCheckin = lastCheckin;
        }
    }
}
