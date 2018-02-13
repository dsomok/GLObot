using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.GLObot.Notifier.Webhook.Commands
{
    public class CommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }
        public string ArgsInput { get; set; }
    }
}