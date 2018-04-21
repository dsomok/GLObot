using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Library.GLO;
using Telegram.Bot.Library.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Commands
{
    abstract class TokenSensitiveCommand : CommandBase<CommandArgs>
    {
        protected readonly IGloOfficeTimeClient _officeTimeClient;

        protected TokenSensitiveCommand(string name, IGloOfficeTimeClient officeTimeClient) 
            : base(name)
        {
            _officeTimeClient = officeTimeClient;
        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, CommandArgs args)
        {
            if (!this._officeTimeClient.IsTokenSet)
            {
                await Bot.Client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "Token should be set.",
                    replyToMessageId: update.Message.MessageId);
                return UpdateHandlingResult.Handled;
            }

            return await this.ExecuteTokenSensitiveInternal(update, args);
        }

        protected abstract Task<UpdateHandlingResult> ExecuteTokenSensitiveInternal(Update update, CommandArgs args);
    }
}
