using System.Threading.Tasks;

namespace Telegram.Bot.GLObot.Notifier.GLO.Employees
{
    interface IEmployeesRegistry
    {
        Task PopulateEmployees();
        Employee GetEmployee(int id);
        void UpdateEmployee(Employee employee);
    }
}