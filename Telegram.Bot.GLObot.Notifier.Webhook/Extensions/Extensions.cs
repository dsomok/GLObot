using System.Threading.Tasks;
using Telegram.Bot.Library.GLO.Checkins;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Extensions
{
    internal static class Extensions
    {
        public static Task SendEmployeeStatistics(this ITelegramBotClient bot, long chatId, string name, CheckinStats checkinStats)
        {
            var direction = checkinStats.LastCheckin.Direction == CheckinDirection.In ? "entering" : "leaving";
            return bot.SendTextMessageAsync(chatId,
                $"Employee *{name}* was last seen *{direction.ToUpperInvariant()}* _{checkinStats.LastCheckin.Area}_ at *{checkinStats.LastCheckin.TimeStampFormated}*" +
                (checkinStats == null ? "" : $"\nWorking time today *{checkinStats.WorkingTimeToday}*") +
                (checkinStats == null ? "" : $"\nTotal time today *{checkinStats.TimeWithTeleports}*") +
                (checkinStats?.FirstCheckinToday == null ? "" : $"\nFirst checkin today *{checkinStats.FirstCheckinFormated}*"),
                ParseMode.Markdown);
        }
    }
}