using System.Threading.Tasks;
using Telegram.Bot.Library.GLO.Checkins;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Extensions
{
    internal static class Extensions
    {
        public static Task SendEmployeeStatistics(this ITelegramBotClient bot, long chatId, string name,
            CheckinDetails checkinDetails, CheckinStats checkinStats = null)
        {
            var direction = checkinDetails.Direction == CheckinDirection.In ? "entering" : "leaving";
            return bot.SendTextMessageAsync(chatId,
                $"Employee *{name}* was last seen *{direction.ToUpperInvariant()}* _{checkinDetails.Area}_ at *{checkinDetails.Timestamp}*" +
                (checkinStats == null ? "" : $"\nTotal time today *{checkinStats.WorkingTimeToday}*.") +
                (checkinStats?.FirstCheckinToday == null ? "" : $"\nFirst checkin today *{checkinStats.FirstCheckinToday}*"),
                ParseMode.Markdown);
        }
    }
}