using Telegram.Bot.Library.Keyboard;

namespace Telegram.Bot.GLObot.Notifier.PredefinedEmployees
{
    class PredefinedEmployeesKeyboard
    {
        public const string AllKey = "Все";

        public KeyboardRow[] Keyboard => new[]
        {
            new KeyboardRow(new[] {"Саня", "Игорь", "Ден"}),
            new KeyboardRow(new[] {"Министр", "Дима"}),
            new KeyboardRow(new[] {AllKey})
        };
    }
}