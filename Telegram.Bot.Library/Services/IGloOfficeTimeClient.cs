using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Library.GLO.Checkins;
using Telegram.Bot.Library.GLO.Employees;

namespace Telegram.Bot.Library.Services
{
    public interface IGloOfficeTimeClient
    {
        void SetToken(string token);
        void OnSetToken(Action action);
        //Task<IList<Employee>> GetEmployees();
        Task<CheckinDetails> WhenLastSeen(int employeeId);
        Task<CheckinStats> TotalOfficeTimeToday(int employeeId);
        bool IsTokenSet { get; }
    }
}