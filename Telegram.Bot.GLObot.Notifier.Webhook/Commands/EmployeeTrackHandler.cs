using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.GLObot.Notifier.Webhook.Extensions;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO;
using Telegram.Bot.GLObot.Notifier.Webhook.PredefinedEmployees;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Commands
{
    internal class EmployeeTrackHandler : UpdateHandlerBase
    {
        private readonly GloOfficeTimeClient _officeTimeClient;
        private readonly PredefinedEmployeesRegistry _predefinedEmployeesRegistry;

        public EmployeeTrackHandler(PredefinedEmployeesRegistry predefinedEmployeesRegistry,
            GloOfficeTimeClient officeTimeClient)
        {
            _predefinedEmployeesRegistry = predefinedEmployeesRegistry;
            _officeTimeClient = officeTimeClient;
        }

        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            return update.Type == UpdateType.CallbackQueryUpdate && update.CallbackQuery != null;
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            var employeeName = update.CallbackQuery.Data;

            var employeeIds = employeeName != PredefinedEmployeesKeyboard.AllKey
                ? new List<int> {_predefinedEmployeesRegistry[employeeName]}
                : _predefinedEmployeesRegistry.Employees.Values.ToList();

            try
            {
                foreach (var employeeId in employeeIds)
                {
                    var name = _predefinedEmployeesRegistry[employeeId];
                    var checkinDetails = await _officeTimeClient.WhenLastSeen(employeeId);
                    var checkinStats = await _officeTimeClient.TotalOfficeTimeToday(employeeId);
                    await bot.Client.SendEmployeeStatistics(update.CallbackQuery.Message.Chat.Id, name, checkinDetails, checkinStats);
                }
            }
            catch (Exception ex)
            {
                await bot.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat, $"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            return await Task.FromResult(UpdateHandlingResult.Handled);
        }
    }
}