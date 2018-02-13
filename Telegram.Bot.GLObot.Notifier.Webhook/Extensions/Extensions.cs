using System.Threading.Tasks;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO.Checkins;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Extensions
{
    internal static class Extensions
    {
        public static Task SendEmployeeStatistics(this ITelegramBotClient bot, long chatId, string name,
            CheckinDetails checkinDetails)
        {
            var direction = checkinDetails.Direction == CheckinDirection.In ? "entering" : "leaving";
            return bot.SendTextMessageAsync(chatId,
                $"Employee *{name}* was last seen *{direction.ToUpperInvariant()}* _{checkinDetails.Area}_ at *{checkinDetails.Timestamp}*",
                ParseMode.Markdown);
        }
    }
}