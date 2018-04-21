using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.GLObot.Notifier.Webhook.Extensions;
using Telegram.Bot.Library.PredefinedEmployees;
using Telegram.Bot.Library.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Commands
{
    internal class EmployeeTrackHandler : UpdateHandlerBase
    {
        private readonly IEmployeesRegistry _employeesRegistry;
        private readonly IGloOfficeTimeClient _officeTimeClient;

        public EmployeeTrackHandler(IEmployeesRegistry employeesRegistry,
            IGloOfficeTimeClient officeTimeClient)
        {
            _employeesRegistry = employeesRegistry;
            _officeTimeClient = officeTimeClient;
        }

        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            return update.Type == UpdateType.CallbackQueryUpdate && update.CallbackQuery != null;
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            var employeeName = update.CallbackQuery.Data;
            var chatId = update.CallbackQuery.Message.Chat.Id;

            var employeeIds = employeeName != PredefinedEmployeesKeyboard.AllKey
                ? new[] {_employeesRegistry.GetEmployeeId(chatId, employeeName)}
                : _employeesRegistry.GetAllEmployeeIds(chatId);

            try
            {
                foreach (var employeeId in employeeIds)
                {
                    var name = _employeesRegistry.GetEmployeeName(chatId, employeeId);
                    var checkinDetails = await _officeTimeClient.WhenLastSeen(employeeId);
                    var checkinStats = await _officeTimeClient.TotalOfficeTimeToday(employeeId);
                    await bot.Client.SendEmployeeStatistics(chatId, name, checkinDetails,
                        checkinStats);
                }
            }
            catch (Exception ex)
            {
                await bot.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat,
                    $"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }

            return await Task.FromResult(UpdateHandlingResult.Handled);
        }
    }
}