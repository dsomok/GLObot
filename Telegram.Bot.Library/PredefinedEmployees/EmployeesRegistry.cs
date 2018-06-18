using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Library.Keyboard;
using Telegram.Bot.Library.Services;

namespace Telegram.Bot.Library.PredefinedEmployees
{
    internal class EmployeesRegistry : IEmployeesRegistry
    {
        private readonly Dictionary<long, List<EmployeeRecord>> _employeeRecords;
        private readonly IEmployeesRepository _employeesRepository;

        public EmployeesRegistry(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
            _employeeRecords = new Dictionary<long, List<EmployeeRecord>>();
        }

        public IEnumerable<KeyboardRow> GetKeyboardRows(long chatId)
        {
            if (!_employeeRecords.ContainsKey(chatId))
                LoadRecords(chatId);

            var employeeRow = new List<string>();
            foreach (var employeeRecord in _employeeRecords[chatId])
            {
                employeeRow.Add(employeeRecord.EmployeeName);
                if (string.Join(',', employeeRow.ToArray()).Length > 12)
                {
                    var keyboardRow = new KeyboardRow(employeeRow.ToArray());
                    employeeRow.Clear();
                    yield return keyboardRow;
                }
            }

            if (employeeRow.Count > 0)
                yield return new KeyboardRow(employeeRow.ToArray());

            yield return new KeyboardRow(new[] {PredefinedEmployeesKeyboard.AllKey});
        }

        public int GetEmployeeId(long chatId, string name)
        {
            if (!_employeeRecords.ContainsKey(chatId))
                LoadRecords(chatId);

            return _employeeRecords[chatId].Single(e => e.EmployeeName == name).EmployeeId;
        }

        public string GetEmployeeName(long chatId, int id)
        {
            if (!_employeeRecords.ContainsKey(chatId))
                LoadRecords(chatId);

            return _employeeRecords[chatId].Single(e => e.EmployeeId == id).EmployeeName;
        }

        public int[] GetAllEmployeeIds(long chatId)
        {
            if (!_employeeRecords.ContainsKey(chatId))
                LoadRecords(chatId);

            return _employeeRecords[chatId].Select(e => e.EmployeeId).ToArray();
        }

        private void LoadRecords(long chatId)
        {
            _employeeRecords.Add(chatId, new List<EmployeeRecord>());
            var overrides = _employeesRepository.GetAll(chatId);
            if (!overrides.Any())
                foreach (var defaultEmployee in PredefinedEmployeesKeyboard.DefaultEmployees)
                {
                    var record = new EmployeeRecord
                    {
                        EmployeeId = defaultEmployee.Value,
                        EmployeeName = defaultEmployee.Key
                    };
                    _employeeRecords[chatId].Add(record);
                    _employeesRepository.Save(chatId, record);
                }
            else
                _employeeRecords[chatId].AddRange(overrides);
        }
    }
}