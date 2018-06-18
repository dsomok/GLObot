using System.Collections.Generic;
using Telegram.Bot.Library.PredefinedEmployees;

namespace Telegram.Bot.Library.Services
{
    internal interface IEmployeesRepository
    {
        IEnumerable<EmployeeRecord> GetAll(long chatId);
        void Save(long chatId, EmployeeRecord employee);
    }
}