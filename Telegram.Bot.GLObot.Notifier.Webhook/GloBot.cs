using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Serilog;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.GLObot.Notifier.Webhook
{
    public class GloBot : BotBase<GloBot>
    {
        private readonly ILogger _logger;

        public GloBot(IOptions<BotOptions<GloBot>> botOptions, ILogger logger) : base(botOptions)
        {
            _logger = logger;
        }

        public override async Task HandleUnknownUpdate(Update update)
        {
            _logger.Warning("Unable to handle update {@Update}", update);
            await Client.SendTextMessageAsync(update.Message.Chat.Id, "Unalbe to handle update", ParseMode.Markdown,
                replyToMessageId: update.Message.MessageId);
        }

        public override async Task HandleFaultedUpdate(Update update, Exception ex)
        {
            _logger.Fatal(ex, "Failed to handle update {@Update}", update);
            await Client.SendTextMessageAsync(update.Message.Chat.Id, "Failed to handle update", ParseMode.Markdown,
                replyToMessageId: update.Message.MessageId);
        }
    }
}