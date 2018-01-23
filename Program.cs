using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Examples.Echo
{
    public static class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("");
        private static ConcurrentDictionary<long, string> _tokenDictionary = new ConcurrentDictionary<long, string>();

        public static void Main(string[] args)
        {
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;

            Bot.StartReceiving();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;

            //IReplyMarkup keyboard = new ReplyKeyboardRemove();

            switch (message.Text.Split(' ').First())
            {
                case "/setToken":
                    if (message.Text.Split(' ').Length == 2)
                    {
                        string token = message.Text.Split(' ')[1];
                        _tokenDictionary.AddOrUpdate(message.Chat.Id, token, ((l, s) => token));
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Wrong format");
                    }

                    break;

                // send inline keyboard
                case "/time":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData("Саня"),
                            InlineKeyboardButton.WithCallbackData("Министр"),
                            InlineKeyboardButton.WithCallbackData("Ден")
                        },
                        new [] // second row
                        {
                            InlineKeyboardButton.WithCallbackData("Дима"),
                            InlineKeyboardButton.WithCallbackData("Игорь"),
                        }
                    });

                    await Bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Choose",
                        replyMarkup: inlineKeyboard);
                    break;
                default:
                    const string usage = @"Usage:
/time   - show time
/setToken - set token
";

                    await Bot.SendTextMessageAsync(
                        message.Chat.Id,
                        usage,
                        replyMarkup: new ReplyKeyboardRemove());
                    break;
            }
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            //await Bot.AnswerCallbackQueryAsync(
            //    callbackQueryEventArgs.CallbackQuery.Id,
            //    $"Received {callbackQueryEventArgs.CallbackQuery.Data}");

            await Bot.SendChatActionAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, ChatAction.Typing);
            if (!_tokenDictionary.TryGetValue(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, out string val))
            {
                await Bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, "Token not found");
                return;
            }

            var client = new RestClient("https://portal-ua.globallogic.com/officetime/json/last_seen.php?zone=KBP&employeeId="
                                                                                        + EmployeeHelper.GetId(callbackQueryEventArgs.CallbackQuery.Data));
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "49b2bdfe-9495-6370-ad67-9e10978a7be8");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("connection", "keep-alive");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("referer", "https://portal-ua.globallogic.com/officetime/");
            request.AddHeader("accept", "*/*");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
            request.AddHeader("accept-language", "ru,en-US;q=0.9,en;q=0.8,uk;q=0.7");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("dnt", "1");
            request.AddHeader("authorization", "Basic " + _tokenDictionary[callbackQueryEventArgs.CallbackQuery.Message.Chat.Id]);

            var response = await client.ExecuteAsync(request);

            var lastSeenData = LastSeenResponse.FromJson(response.Content);

            await Bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, callbackQueryEventArgs.CallbackQuery.Data + " last seen in " + lastSeenData.Area + " " +
                                                                                     lastSeenData.DiffStr + " | " + lastSeenData.Timestamp);
        }


        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }
    }

    public static class RestClientExtensions
    {
        public static async Task<RestResponse> ExecuteAsync(this RestClient client, RestRequest request)
        {
            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));
            return (RestResponse)(await taskCompletion.Task);
        }
    }


}
