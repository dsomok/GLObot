using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.GLObot.Notifier.GLO;
using Telegram.Bot.Library;
using Telegram.Bot.Library.Commands;
using Telegram.Bot.Library.Commands.Annotations;
using Telegram.Bot.Library.Exceptions;

namespace Telegram.Bot.GLObot.Notifier.Commands
{
    [BotCommand("/setToken")]
    class SetTokenCommand : BotCommand<string>
    {
        private readonly GLOOfficeTimeClient _officeTimeClient;

        public SetTokenCommand(TelegramBot bot, GLOOfficeTimeClient officeTimeClient) : base(bot)
        {
            _officeTimeClient = officeTimeClient;
        }

        protected override bool TryParseArgs(IEnumerable<string> args, out string parsedArgs)
        {
            if (args.Count() != 1)
            {
                throw new BotCommandException($"{this.Name} command accepts only 1 argument.");
            }

            parsedArgs = args.First();
            return true;
        }

        protected override Task<string> ExecuteInternal(long chatId, string token)
        {
            var isTokenSet = this._officeTimeClient.IsTokenSet;
            this._officeTimeClient.SetToken(token);

            return isTokenSet
                ? Task.FromResult("Token was successfully updated.")
                : Task.FromResult("Token was successfully set.");
        }
    }
}
