using System.Collections.Generic;
using System.Threading.Tasks;

namespace Telegram.Bot.Library.Commands
{
    public interface IBotCommandExecutor
    {
        Task<string> Execute(long chatId, string commandName, IEnumerable<string> args);
    }
}