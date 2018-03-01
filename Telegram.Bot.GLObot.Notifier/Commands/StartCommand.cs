using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.GLObot.Notifier.Extensions;
using Telegram.Bot.GLObot.Notifier.GLO;
using Telegram.Bot.GLObot.Notifier.PredefinedEmployees;
using Telegram.Bot.Library;
using Telegram.Bot.Library.Commands.Annotations;
using Telegram.Bot.Library.Keyboard;

namespace Telegram.Bot.GLObot.Notifier.Commands
{
    [BotCommand("/start")]
    class StartCommand : TokenSensitiveCommand
    {
        private readonly GLOOfficeTimeClient _officeTimeClient;
        private readonly PredefinedEmployeesRegistry _predefinedEmployeesRegistry;
        private readonly PredefinedEmployeesKeyboard _predefinedEmployeesKeyboard;


        public StartCommand(
            TelegramBot bot, 
            GLOOfficeTimeClient officeTimeClient, 
            PredefinedEmployeesRegistry predefinedEmployeesRegistry, 
            PredefinedEmployeesKeyboard predefinedEmployeesKeyboard
        )
            : base(bot, officeTimeClient)
        {
            _officeTimeClient = officeTimeClient;
            _predefinedEmployeesRegistry = predefinedEmployeesRegistry;
            _predefinedEmployeesKeyboard = predefinedEmployeesKeyboard;
        }



        protected override Task<string> ExecuteTokenSensitiveInternal(long chatId)
        {
            return this.Bot.ShowInlineKeyboard(chatId, "Choose Employee", this._predefinedEmployeesKeyboard.Keyboard, async employeeName =>
            {
                var employeeIds = employeeName != PredefinedEmployeesKeyboard.AllKey
                    ? new List<int>{ this._predefinedEmployeesRegistry[employeeName] } 
                    : this._predefinedEmployeesRegistry.Employees.Values.ToList();

                foreach (var employeeId in employeeIds)
                {
                    var name = this._predefinedEmployeesRegistry[employeeId];
                    var checkinDetails = await this._officeTimeClient.WhenLastSeen(employeeId);
                    var checkinStats = await this._officeTimeClient.TotalOfficeTimeToday(employeeId);
                    await this.Bot.SendEmployeeStatistics(chatId, name, checkinDetails, checkinStats);
                }
            });
        }
    }
}
