using System.Collections.Generic;
using System.Linq;

namespace Telegram.Bot.GLObot.Notifier.PredefinedEmployees
{
    class PredefinedEmployeesRegistry
    {
        public PredefinedEmployeesRegistry()
        {
            this.Employees = new Dictionary<string, int>
            {
                {"Игорь", 6583},
                {"Министр", 5502},
                {"Ден", 6579},
                {"Саня", 6584},
                {"Дима", 6499}
            };
        }

        public IReadOnlyDictionary<string, int> Employees { get; }

        public int this[string index] => this.Employees[index];
        public string this[int index] => this.Employees.SingleOrDefault(kv => kv.Value == index).Key;
    }
}
