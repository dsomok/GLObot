using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO;
using Telegram.Bot.GLObot.Notifier.Webhook.PredefinedEmployees;
using Telegram.Bot.Library.Keyboard;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Commands
{
    internal class StartCommand : TokenSensitiveCommand
    {
        private readonly ILogger _logger;
        private readonly PredefinedEmployeesKeyboard _predefinedEmployeesKeyboard;

        public StartCommand(GloOfficeTimeClient officeTimeClient,
            PredefinedEmployeesKeyboard predefinedEmployeesKeyboard,
            ILogger logger) : base("start", officeTimeClient)
        {
            _logger = logger;
            _predefinedEmployeesKeyboard = predefinedEmployeesKeyboard;
        }

        protected override async Task<UpdateHandlingResult> ExecuteTokenSensitiveInternal(Update update,
            CommandArgs args)
        {
            await ShowInlineKeyboard(update.Message.Chat.Id, "Choose Employee",
                _predefinedEmployeesKeyboard.Keyboard);

            return UpdateHandlingResult.Continue;
        }

        private async Task<string> ShowInlineKeyboard(long chatId, string title, KeyboardRow[] rows)
        {
            _logger.Information("Showing keyboard with values {@Keyboard} to chat {ChatId}",
                rows.SelectMany(r => r.Values), chatId);

            var inlineKeyboardButtons =
                rows.Select(r => r.Values.Select(InlineKeyboardButton.WithCallbackData).ToArray()).ToArray();

            await Bot.Client.SendTextMessageAsync(
                new ChatId(chatId),
                title,
                replyMarkup: new InlineKeyboardMarkup(inlineKeyboardButtons)
            );

            return null;
        }
    }
}