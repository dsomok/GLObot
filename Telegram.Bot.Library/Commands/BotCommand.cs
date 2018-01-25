using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Library.Commands.Annotations;
using Telegram.Bot.Library.Exceptions;

namespace Telegram.Bot.Library.Commands
{
    public abstract class BotCommand<TArgs> : IBotCommand
    {
        protected BotCommand(TelegramBot bot)
        {
            Bot = bot;

            this.Name = this.GetType().GetCustomAttribute<BotCommandAttribute>()?.Name;
        }

        protected TelegramBot Bot { get; }

        public string Name { get; }

        public Task<string> Execute(long chatId, IEnumerable<string> args)
        {
            if (!this.TryParseArgs(args, out TArgs parsedArgs))
            {
                throw new BotCommandException($"Invalid args were provided for {this.Name} command");
            }

            return this.ExecuteInternal(chatId, parsedArgs);
        }

        protected abstract bool TryParseArgs(IEnumerable<string> args, out TArgs parsedArgs);

        protected abstract Task<string> ExecuteInternal(long chatId, TArgs args);
    }

    public abstract class BotCommand : IBotCommand
    {
        protected BotCommand(TelegramBot bot)
        {
            Bot = bot;

            this.Name = this.GetType().GetCustomAttribute<BotCommandAttribute>()?.Name;
        }

        protected TelegramBot Bot { get; }

        public string Name { get; }

        public Task<string> Execute(long chatId, IEnumerable<string> args)
        {
            if (args.Any())
            {
                throw new BotCommandException($"{this.Name} command doesn't accept any arguments");
            }

            return this.ExecuteInternal(chatId);
        }

        protected abstract Task<string> ExecuteInternal(long chatId);
    }
}