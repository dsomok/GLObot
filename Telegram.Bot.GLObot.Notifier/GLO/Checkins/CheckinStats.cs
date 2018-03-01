using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.GLObot.Notifier.GLO.Checkins
{
    public class CheckinStats
    {
        public CheckinStats(TimeSpan workingTimeToday, byte teleportsCount, DateTime? firstCheckinToday)
        {
            WorkingTimeToday = workingTimeToday;
            TeleportsCount = teleportsCount;
            FirstCheckinToday = firstCheckinToday;
        }
        public TimeSpan WorkingTimeToday { get; private set; }
        public byte TeleportsCount { get; private set; }
        public DateTime? FirstCheckinToday { get; private set; }
    }
}
