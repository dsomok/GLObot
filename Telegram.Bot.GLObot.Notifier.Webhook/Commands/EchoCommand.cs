using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Commands
{
    public class EchoCommand : CommandBase<CommandArgs>
    {
        public EchoCommand() : base("echo")
        {
        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, CommandArgs args)
        {
            var replyText = string.IsNullOrWhiteSpace(args.ArgsInput) ? "Echo What?" : args.ArgsInput;

            await Bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                replyText,
                replyToMessageId: update.Message.MessageId);

            return UpdateHandlingResult.Handled;
        }
    }
}