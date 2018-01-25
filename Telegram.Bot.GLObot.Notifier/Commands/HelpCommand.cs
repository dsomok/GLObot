using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Library;
using Telegram.Bot.Library.Commands;
using Telegram.Bot.Library.Commands.Annotations;

namespace Telegram.Bot.GLObot.Notifier.Commands
{
    [BotCommand("/help")]
    class HelpCommand : BotCommand
    {
        private readonly IBotCommandsFactory _commandsFactory;

        public HelpCommand(TelegramBot bot, IBotCommandsFactory commandsFactory)
            : base(bot)
        {
            _commandsFactory = commandsFactory;
        }

        protected override Task<string> ExecuteInternal(long chatId)
        {
            var helpMessage = this._commandsFactory
                                  .KnownCommands
                                  .Aggregate((seed, commandName) => $"{seed}{Environment.NewLine}{commandName}");

            return Task.FromResult(helpMessage);
        }
    }
}