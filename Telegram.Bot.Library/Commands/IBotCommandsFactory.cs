using System.Collections.Generic;

namespace Telegram.Bot.Library.Commands
{
    public interface IBotCommandsFactory
    {
        IReadOnlyList<string> KnownCommands { get; }
        IBotCommand Create(string commandName);
    }
}