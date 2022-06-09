﻿using System;
using System.Threading.Tasks;
using Serilog;
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
        private readonly ILogger _logger;

        public EmployeeTrackHandler(IEmployeesRegistry employeesRegistry,
            IGloOfficeTimeClient officeTimeClient,
            ILogger logger)
        {
            _logger = logger;
            _employeesRegistry = employeesRegistry;
            _officeTimeClient = officeTimeClient;
        }

        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            var plainTextRequest = update.Message.Text.StartsWith('#');
            return update.Type == UpdateType.CallbackQueryUpdate && update.CallbackQuery != null || plainTextRequest;
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            var plainTextRequest = update.Message.Text.StartsWith('#');
            Chat chat = plainTextRequest ? update.Message.Chat : update.CallbackQuery.Message.Chat;
            string employeeName = plainTextRequest ? update.Message.Text.TrimStart('#') : update.CallbackQuery.Data;
            
            var chatId = chat.Id;
            _logger.Information("Handling track command for employee {employees}", employeeName);
            var employeeIds = employeeName != PredefinedEmployeesKeyboard.AllKey
                ? new[] {_employeesRegistry.GetEmployeeId(chatId, employeeName)}
                : _employeesRegistry.GetAllEmployeeIds(chatId);
            _logger.Information("Handling track command for employee(s) {employees}", string.Join(",", employeeIds));
            try
            {
                foreach (var employeeId in employeeIds)
                {
                    var name = _employeesRegistry.GetEmployeeName(chatId, employeeId);
                    var checkinStats = await _officeTimeClient.TotalOfficeTimeToday(employeeId);
                    if (checkinStats.LastCheckin == null)
                    {
                        var checkinDetails = await _officeTimeClient.WhenLastSeen(employeeId);
                        checkinStats.UpdateLastCheckin(checkinDetails);
                    }

                    await bot.Client.SendEmployeeStatistics(chatId, name, checkinStats);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to handle track command for employee(s) {employees}", string.Join(",", employeeIds));
                await bot.Client.SendTextMessageAsync(chat,
                    $"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            _logger.Information("Handled track command for employee(s) {employees}", string.Join(",", employeeIds));
            return await Task.FromResult(UpdateHandlingResult.Handled);
        }
    }
}