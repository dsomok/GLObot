using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.GLObot.Notifier.GLO;
using Telegram.Bot.GLObot.Notifier.GLO.Checkins;
using Telegram.Bot.Library;
using Telegram.Bot.Library.Commands.Annotations;
using Telegram.Bot.Library.Exceptions;

namespace Telegram.Bot.GLObot.Notifier.Commands
{
    [BotCommand("/stop")]
    class StopCommand : TokenSensitiveCommand<int>
    {
        private readonly GLOOfficeTimePollerRegistry _pollerRegistry;

        public StopCommand(
            TelegramBot bot, 
            GLOOfficeTimePollerRegistry pollerRegistry,
            GLOOfficeTimeClient officeTimeClient
        ) : base(bot, officeTimeClient)
        {
            _pollerRegistry = pollerRegistry;
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
            this._pollerRegistry.StopPoller(employeeId);
            return Task.FromResult($"Stopped polling for employee {employeeId}");
        }
    }
}