using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Library.PredefinedEmployees;
using Telegram.Bot.Library.Services;

namespace Telegram.Bot.Library.Repositories
{
    internal class InmemoryEmployeesRepository : IEmployeesRepository
    {
        private readonly EmployeeOverrideTable _dbMock;

        public InmemoryEmployeesRepository()
        {
            _dbMock = new EmployeeOverrideTable();
        }

        public IEnumerable<EmployeeRecord> GetAll(long chatId)
        {
            return _dbMock.EmployeeOverrides.Where(e => e.ChatId == chatId)
                .Select(e => new EmployeeRecord(){EmployeeId = e.EmployeeId, EmployeeName = e.EmployeeName})
                .ToList();

        }

        public void Save(long chatId, EmployeeRecord employee)
        {
            _dbMock.Add(new EmployeeOverrideRecord(){ChatId = chatId, EmployeeId = employee.EmployeeId, EmployeeName = employee.EmployeeName});
        }

        private class EmployeeOverrideTable
        {
            public EmployeeOverrideTable()
            {
                _table = new List<EmployeeOverrideRecord>();
            }

            private readonly List<EmployeeOverrideRecord> _table;
            public IQueryable<EmployeeOverrideRecord> EmployeeOverrides => _table.AsQueryable();

            public void Add(EmployeeOverrideRecord record)
            {
                _table.Add(record);
            }
        }

        private class EmployeeOverrideRecord
        {
            public long ChatId { get; set; }
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; }
        }
    }
}