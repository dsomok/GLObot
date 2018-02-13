using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO.Checkins;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO.Employees;

namespace Telegram.Bot.GLObot.Notifier.Webhook.GLO.Serialization
{
    interface IDeserializer
    {
        IList<Employee> DeserializeEmployees(string json);
        CheckinDetails DeserializeCheckinDetails(string json);
    }

    class Deserializer : IDeserializer
    {
        public IList<Employee> DeserializeEmployees(string json)
        {
            var employeesArray = JsonConvert.DeserializeObject<IList<dynamic>>(json);

            var employees = employeesArray.Where(e => e.zone == "KBP")
                                          .Select(e => new Employee(
                                              id: (int) e.uid,
                                              name: $"{e.first_name} {e.last_name}",
                                              timestamp: string.Empty))
                                          .ToList();
            return employees;
        }

        public CheckinDetails DeserializeCheckinDetails(string json)
        {
            dynamic responseBody = JsonConvert.DeserializeObject(json);
            if (responseBody == null)
            {
                return null;
            }
            
            return new CheckinDetails(
                area: (string)responseBody.area,
                secondsAgo: TimeSpan.FromSeconds((int)responseBody.diff_s), 
                direction: responseBody.direction == "in" ? CheckinDirection.In : CheckinDirection.Out,
                timestamp: (string)responseBody.timestamp
            );
        }
    }
}
