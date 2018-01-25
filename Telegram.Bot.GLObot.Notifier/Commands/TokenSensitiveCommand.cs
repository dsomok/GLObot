using System.Threading.Tasks;
using Telegram.Bot.GLObot.Notifier.GLO;
using Telegram.Bot.Library;
using Telegram.Bot.Library.Commands;

namespace Telegram.Bot.GLObot.Notifier.Commands
{
    abstract class TokenSensitiveCommand<TArg> : BotCommand<TArg>
    {
        private readonly GLOOfficeTimeClient _officeTimeClient;

        protected TokenSensitiveCommand(TelegramBot bot, GLOOfficeTimeClient officeTimeClient) 
            : base(bot)
        {
            _officeTimeClient = officeTimeClient;
        }
        
        protected sealed override Task<string> ExecuteInternal(long chatId, TArg args)
        {
            if (!this._officeTimeClient.IsTokenSet)
            {
                return Task.FromResult($"Token should be set.");
            }

            return this.ExecuteTokenSensitiveInternal(chatId, args);
        }

        protected abstract Task<string> ExecuteTokenSensitiveInternal(long chatId, TArg args);
    }

    abstract class TokenSensitiveCommand : BotCommand
    {
        private readonly GLOOfficeTimeClient _officeTimeClient;

        protected TokenSensitiveCommand(TelegramBot bot, GLOOfficeTimeClient officeTimeClient) : base(bot)
        {
            _officeTimeClient = officeTimeClient;
        }

        protected sealed override Task<string> ExecuteInternal(long chatId)
        {
            if (!this._officeTimeClient.IsTokenSet)
            {
                return Task.FromResult($"Token should be set.");
            }

            return this.ExecuteTokenSensitiveInternal(chatId);
        }

        protected abstract Task<string> ExecuteTokenSensitiveInternal(long chatId);
    }
}
