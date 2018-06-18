using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Library.GLO;
using Telegram.Bot.Library.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Commands
{
    public class SetTokenCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }

        public string ArgsInput { get; set; }
    }

    internal class SetTokenCommand : CommandBase<SetTokenCommandArgs>
    {
        private readonly IGloOfficeTimeClient _officeTimeClient;

        public SetTokenCommand(IGloOfficeTimeClient officeTimeClient) : base("setToken")
        {
            _officeTimeClient = officeTimeClient;
        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, SetTokenCommandArgs args)
        {
            var isTokenSet = _officeTimeClient.IsTokenSet;
            _officeTimeClient.SetToken(args.ArgsInput);

            var replyText = isTokenSet
                ? "Token was successfully updated."
                : "Token was successfully set.";

            await Bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                replyText,
                replyToMessageId: update.Message.MessageId);

            return UpdateHandlingResult.Handled;
        }
    }
}