using System.Threading.Tasks;

namespace Telegram.Bot.Library.GLO.Employees
{
    internal interface IEmployeesRegistry
    {
        Task PopulateEmployees();
        Employee GetEmployee(int id);
        void UpdateEmployee(Employee employee);
    }
}