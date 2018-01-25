using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.GLObot.Notifier.Extensions;
using Telegram.Bot.GLObot.Notifier.GLO;
using Telegram.Bot.GLObot.Notifier.GLO.Checkins;
using Telegram.Bot.GLObot.Notifier.GLO.Employees;
using Telegram.Bot.GLObot.Notifier.PredefinedEmployees;
using Telegram.Bot.Library;
using Telegram.Bot.Library.Commands.Annotations;
using Telegram.Bot.Library.Exceptions;

namespace Telegram.Bot.GLObot.Notifier.Commands
{
    [BotCommand("/track")]
    class TrackCommand : TokenSensitiveCommand<int>
    {
        private readonly GLOOfficeTimePollerRegistry _pollerRegistry;
        private readonly IEmployeesRegistry _employeesRegistry;
        private readonly PredefinedEmployeesRegistry _predefinedEmployeesRegistry;
        private readonly PredefinedEmployeesKeyboard _predefinedEmployeesKeyboard;

        public TrackCommand(
            TelegramBot bot,
            GLOOfficeTimePollerRegistry pollerRegistry,
            IEmployeesRegistry employeesRegistry,
            GLOOfficeTimeClient officeTimeClient, 
            PredefinedEmployeesRegistry predefinedEmployeesRegistry, 
            PredefinedEmployeesKeyboard predefinedEmployeesKeyboard
        )
            : base(bot, officeTimeClient)
        {
            _pollerRegistry = pollerRegistry;
            _employeesRegistry = employeesRegistry;
            _predefinedEmployeesRegistry = predefinedEmployeesRegistry;
            _predefinedEmployeesKeyboard = predefinedEmployeesKeyboard;
        }

        protected override bool TryParseArgs(IEnumerable<string> args, out int employeeId)
        {
            if (args.Count() > 1)
            {
                throw new BotCommandException($"{this.Name} command accepts only 0 or 1 argument.");
            }

            if (!args.Any())
            {
                employeeId = -1;
                return true;
            }

            return int.TryParse(args.First(), out employeeId);
        }

        protected override async Task<string> ExecuteTokenSensitiveInternal(long chatId, int employeeId)
        {
            if (employeeId != -1)
            {
                return await this.ExecuteForEmployee(chatId, employeeId);
            }
            
            return await this.Bot.ShowInlineKeyboard(chatId, "Choose Employee", this._predefinedEmployeesKeyboard.Keyboard, async employeeName =>
            {
                var employeeIds = employeeName != PredefinedEmployeesKeyboard.AllKey
                    ? new List<int> { this._predefinedEmployeesRegistry[employeeName] }
                    : this._predefinedEmployeesRegistry.Employees.Values.ToList();

                foreach (var id in employeeIds)
                {
                    var result = await this.ExecuteForEmployee(chatId, id);
                    await this.Bot.SendMessage(chatId, result);
                }
            });
        }


        private Task<string> ExecuteForEmployee(long chatId, int employeeId)
        {
            var employee = this._employeesRegistry.GetEmployee(employeeId);

            this._pollerRegistry.StartPoller(employeeId, async checkinDetails =>
            {
                if (checkinDetails.Timestamp != employee.Timestamp)
                {
                    await this.Bot.SendEmployeeStatistics(chatId, employee.Name, checkinDetails);

                    employee.UpdateTimestamp(checkinDetails.Timestamp);
                    this._employeesRegistry.UpdateEmployee(employee);
                }
            });

            return Task.FromResult($"Started polling for employee {employeeId}");
        }
    }
}
