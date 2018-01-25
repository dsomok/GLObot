using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Examples.Echo
{
    public static class EmployeeHelper
    {
        private static ConcurrentDictionary<string, long> _employeesDictionary = new ConcurrentDictionary<string, long> ( new List<KeyValuePair<string, long>>
        {
            new KeyValuePair<string, long> ("Игорь", 6583),
            new KeyValuePair<string, long> ("Министр", 5502),
            new KeyValuePair<string, long> ("Ден", 6579),
            new KeyValuePair<string, long> ("Саня", 6584),
            new KeyValuePair<string, long> ("Дима", 6499)

        } );
        public static long GetId(string name)
        {
            _employeesDictionary.TryGetValue(name, out long id);
            return id;
        }
    }
}
