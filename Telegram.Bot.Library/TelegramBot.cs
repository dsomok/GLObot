using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot.Args;
using Telegram.Bot.Library.Commands;
using Telegram.Bot.Library.Keyboard;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Library
{
    public class TelegramBot : IDisposable
    {
        private readonly TelegramBotClient _telegramBot;
        private readonly IBotCommandExecutor _commandExecutor;
        private readonly ILogger _logger;



        public TelegramBot(string token, IBotCommandExecutor commandExecutor, ILogger logger)
        {
            _commandExecutor = commandExecutor;
            _logger = logger;

            _telegramBot = new TelegramBotClient(token);
            _telegramBot.OnMessage += OnMessageReceived;
            _telegramBot.OnMessageEdited += OnMessageReceived;
            //_telegramBot.OnCallbackQuery += BotOnCallbackQueryReceived;
            //_telegramBot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            _telegramBot.OnReceiveError += OnError;
        }



        public void Start()
        {
            this._logger.Information("Started bot");
            this._telegramBot.StartReceiving();
        }

        public Task SendMessage(long chatId, string message)
        {
            this._logger.Information("Sending message {Message} to chat {ChatId}", message, chatId);
            return this._telegramBot.SendTextMessageAsync(new ChatId(chatId), message, ParseMode.Markdown);
        }

        public async Task<string> ShowInlineKeyboard(long chatId, string title, KeyboardRow[] rows, Func<string, Task> callbackAction)
        {
            this._logger.Information("Showing keyboard with values {@Keyboard} to chat {ChatId}", rows.SelectMany(r => r.Values), chatId);

            var inlineKeyboardButtons = rows.Select(r => r.Values.Select(InlineKeyboardButton.WithCallbackData));

            await this._telegramBot.SendTextMessageAsync(
                new ChatId(chatId),
                title,
                replyMarkup: new InlineKeyboardMarkup(inlineKeyboardButtons)
            );

            async void Callback(object s, CallbackQueryEventArgs args)
            {
                if (args.CallbackQuery.Message.Chat.Id != chatId)
                    return;

                await callbackAction(args.CallbackQuery.Data);

                this._telegramBot.OnCallbackQuery -= Callback;
            }

            this._telegramBot.OnCallbackQuery += Callback;

            return null;
        }



        private async void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var chatId = messageEventArgs.Message.Chat.Id;
            var splittedCommand = messageEventArgs.Message.Text.Split(' ');
            var commandName = splittedCommand.First();
            var args = splittedCommand.Skip(1).ToList();

            this._logger.Information("Received command {Command} from chat {ChatId} with args {@Args}", commandName, chatId, args);

            var result = await this._commandExecutor.Execute(chatId, commandName, args);

            if (!string.IsNullOrEmpty(result))
            {
                await this.SendMessage(chatId, result);
            }
        }

        private void OnError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            this._logger.Information("Received error: {ErrorCode} — {ErrorMessage}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }



        public void Dispose()
        {
            this._logger.Information("Stopping bot...");
            this._telegramBot?.StopReceiving();
        }
    }
}