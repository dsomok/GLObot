using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.GLObot.Notifier.Extensions;
using Telegram.Bot.GLObot.Notifier.GLO;
using Telegram.Bot.GLObot.Notifier.GLO.Checkins;
using Telegram.Bot.GLObot.Notifier.GLO.Employees;
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

        public TrackCommand(
            TelegramBot bot,
            GLOOfficeTimePollerRegistry pollerRegistry,
            IEmployeesRegistry employeesRegistry,
            GLOOfficeTimeClient officeTimeClient
        )
            : base(bot, officeTimeClient)
        {
            _pollerRegistry = pollerRegistry;
            _employeesRegistry = employeesRegistry;
        }

        protected override bool TryParseArgs(IEnumerable<string> args, out int parsedArgs)
        {
            if (args.Count() != 1)
            {
                throw new BotCommandException($"{this.Name} command accepts only 1 argument.");
            }

            return int.TryParse(args.First(), out parsedArgs);
        }

        protected override Task<string> ExecuteTokenSensitiveInternal(long chatId, int employeeId)
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
