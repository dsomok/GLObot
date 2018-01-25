using System.Collections.Generic;
using System.Linq;

namespace Telegram.Bot.Library.Keyboard
{
    public class KeyboardRow
    {
        public KeyboardRow(IEnumerable<string> values)
        {
            Values = values.ToList();
        }

        public IReadOnlyList<string> Values { get; }
    }
}
