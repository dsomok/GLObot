using System;

namespace Telegram.Bot.Library.Exceptions
{
    public class BotCommandException : Exception
    {
        public BotCommandException(string message) : base(message)
        {
        }
    }
}
