using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace Telegram.Bot.Library.GLO.Employees
{
    class EmployeesRegistry : IEmployeesRegistry
    {
        private readonly GloOfficeTimeClient _client;
        private readonly ILogger _logger;

        private ConcurrentDictionary<int, Employee> _employees;

        public EmployeesRegistry(GloOfficeTimeClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task PopulateEmployees()
        {
            if (this._employees != null)
            {
                return;
            }

            this._logger.Information("Started populating of employees");

            var employees = (await this._client.GetEmployees()).ToDictionary(e => e.Id, e => e);

            this._employees = new ConcurrentDictionary<int, Employee>(employees);

            this._logger.Information("Finished populating of employees");
        }

        public Employee GetEmployee(int id)
        {
            if (this._employees == null)
            {
                throw new NoNullAllowedException("Employees list is not populated");
            }

            if (!this._employees.TryGetValue(id, out Employee employee))
            {
                throw new KeyNotFoundException($"Employee with id {id} was not found");
            }

            return employee;
        }

        public void UpdateEmployee(Employee employee)
        {
            if (this._employees == null)
            {
                throw new NoNullAllowedException("Employees list is not populated");
            }

            this._employees.TryUpdate(employee.Id, employee, employee);
        }
    }
}
