using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot.Library.Exceptions;

namespace Telegram.Bot.Library.Commands
{
    internal class BotCommandExecutor : IBotCommandExecutor
    {
        private readonly IBotCommandsFactory _commandsFactory;
        private readonly ILogger _logger;

        public BotCommandExecutor(IBotCommandsFactory commandsFactory, ILogger logger)
        {
            _commandsFactory = commandsFactory;
            _logger = logger;
        }

        public Task<string> Execute(long chatId, string commandName, IEnumerable<string> args)
        {
            var command = this._commandsFactory.Create(commandName);
            if (command != null)
            {
                try
                {
                    return command.Execute(chatId, args);
                }
                catch (BotCommandException ex)
                {
                    return Task.FromResult<string>(ex.Message);
                }
                catch (Exception ex)
                {
                    this._logger.Error("Failed to process command {Command}. Exception: {@Exception}", commandName, ex);
                    return Task.FromResult("Failed to process command due to internal error.");
                }
            }

            return Task.FromResult("Unknown command. Type /help to get the list of supported commands");
        }
    }
}