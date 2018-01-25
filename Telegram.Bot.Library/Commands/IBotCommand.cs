using System.Collections.Generic;
using System.Threading.Tasks;

namespace Telegram.Bot.Library.Commands
{
    public interface IBotCommand
    {
        string Name { get; }

        Task<string> Execute(long chatId, IEnumerable<string> args);
    }
}