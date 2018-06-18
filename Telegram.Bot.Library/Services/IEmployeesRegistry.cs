using System.Collections.Generic;
using Telegram.Bot.Library.Keyboard;

namespace Telegram.Bot.Library.Services
{
    public interface IEmployeesRegistry
    {
        IEnumerable<KeyboardRow> GetKeyboardRows(long chatId);
        int GetEmployeeId(long chatId, string name);
        string GetEmployeeName(long chatId, int id);
        int[] GetAllEmployeeIds(long chatId);
    }
}