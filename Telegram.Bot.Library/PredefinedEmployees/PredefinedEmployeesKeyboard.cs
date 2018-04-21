using System.Collections.Generic;

namespace Telegram.Bot.Library.PredefinedEmployees
{
    public class PredefinedEmployeesKeyboard
    {
        public const string AllKey = "Everyone";

        internal static IReadOnlyDictionary<string, int> DefaultEmployees = new Dictionary<string, int>
        {
            {"Игорь", 6583},
            {"Министр", 5502},
            {"Ден", 6579},
            {"Саня", 6584},
            {"Дима", 6499}
        };
    }
}