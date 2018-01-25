using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.GLObot.Notifier.Extensions;
using Telegram.Bot.GLObot.Notifier.GLO;
using Telegram.Bot.Library;
using Telegram.Bot.Library.Commands;
using Telegram.Bot.Library.Commands.Annotations;
using Telegram.Bot.Library.Keyboard;

namespace Telegram.Bot.GLObot.Notifier.Commands
{
    [BotCommand("/start")]
    class StartCommand : TokenSensitiveCommand
    {
        private readonly GLOOfficeTimeClient _officeTimeClient;

        private readonly IDictionary<string, int> _knownEmployees = new Dictionary<string, int>
        {
            {"Игорь", 6583},
            {"Министр", 5502},
            {"Ден", 6579},
            {"Саня", 6584},
            {"Дима", 6499}
        };



        public StartCommand(TelegramBot bot, GLOOfficeTimeClient officeTimeClient)
            : base(bot, officeTimeClient)
        {
            _officeTimeClient = officeTimeClient;
        }



        protected override Task<string> ExecuteTokenSensitiveInternal(long chatId)
        {
            var rows = new[]
            {
                new KeyboardRow(new[] {"Саня", "Игорь", "Ден"}),
                new KeyboardRow(new[] {"Министр", "Дима"})
            };

            return this.Bot.ShowInlineKeyboard(chatId, "Choose Employee", rows, async employeeName =>
            {
                var employeeId = this._knownEmployees[employeeName];
                var checkinDetails = await this._officeTimeClient.WhenLastSeen(employeeId);

                await this.Bot.SendEmployeeStatistics(chatId, employeeName, checkinDetails);
            });
        }
    }
}
