using System.Collections.Generic;

namespace Telegram.Bot.Library.PredefinedEmployees
{
    public class PredefinedEmployeesKeyboard
    {
        public const string AllKey = "Everyone";

        internal static IReadOnlyDictionary<string, int> DefaultEmployees = new Dictionary<string, int>
        {
            {"Саня", 6584},
            {"Игорь", 6583},
            {"Ден", 6579},
            {"Саша", 6703},
            {"Дима", 6499},
            {"Ира", 552 }
        };
    }
}