using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Library.Keyboard;
using Telegram.Bot.Library.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Commands
{
    internal class KeyboardMarkupCommand : TokenSensitiveCommand
    {
        private readonly IEmployeesRegistry _employeesRegistry;
        private readonly ILogger _logger;

        public KeyboardMarkupCommand(IGloOfficeTimeClient officeTimeClient,
            ILogger logger, IEmployeesRegistry employeesRegistry) : base("keyboard", officeTimeClient)
        {
            _logger = logger;
            _employeesRegistry = employeesRegistry;
        }

        protected override async Task<UpdateHandlingResult> ExecuteTokenSensitiveInternal(Update update,
            CommandArgs args)
        {
            await ShowKeyboardMarkup(update.Message.Chat.Id, "Choose Employee",
                _employeesRegistry.GetKeyboardMarkup(update.Message.Chat.Id).ToArray());

            return UpdateHandlingResult.Continue;
        }

        private async Task<string> ShowKeyboardMarkup(long chatId, string title, KeyboardButton[][] buttons)
        {
            _logger.Information("Showing keyboard markup with values {@Keyboard} to chat {ChatId}",
                buttons.SelectMany(c => c.Select(r => r.Text)), chatId);

            var rkm = new ReplyKeyboardMarkup
            {
                Keyboard = buttons
            };

            await Bot.Client.SendTextMessageAsync(
                new ChatId(chatId),
                title,
                replyMarkup: rkm
            );

            return null;
        }
    }
}