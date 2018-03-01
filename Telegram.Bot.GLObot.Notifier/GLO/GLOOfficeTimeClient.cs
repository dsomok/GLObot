using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.GLObot.Notifier.GLO.Checkins;
using Telegram.Bot.GLObot.Notifier.GLO.Employees;
using Telegram.Bot.GLObot.Notifier.GLO.Serialization;
using Telegram.Bot.Library.Exceptions;

namespace Telegram.Bot.GLObot.Notifier.GLO
{
    class GLOOfficeTimeClient
    {
        private readonly IDeserializer _deserializer;
        private readonly HttpClient _httpClient;

        private string _token;
        private readonly IList<Action> _onSetTokenActions = new List<Action>();



        public GLOOfficeTimeClient(IDeserializer deserializer)
        {
            _deserializer = deserializer;
            this._httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://portal-ua.globallogic.com/officetime/json/")
            };
        }



        public bool IsTokenSet => !string.IsNullOrEmpty(this._token);



        public void SetToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new BotCommandException("Token should not be empty");
            }

            if (!this.IsTokenSet)
            {
                this._token = token;
                this._httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");

                foreach (var onSetTokenAction in this._onSetTokenActions)
                {
                    onSetTokenAction();
                }
            }
            else
            {
                this._token = token;
                this._httpClient.DefaultRequestHeaders.Remove("Authorization");
                this._httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");
            }
        }

        public void OnSetToken(Action action)
        {
            if (!string.IsNullOrEmpty(this._token))
            {
                throw new InvalidOperationException("Token was already set.");
            }

            this._onSetTokenActions.Add(action);
        }


        
        public async Task<IList<Employee>> GetEmployees()
        {
            this.CheckToken();

            var relativePath = "employees.php?zone=KBP";
            var response = await this._httpClient.GetAsync(relativePath);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Failed to load employees");
            }

            var json = await response.Content.ReadAsStringAsync();
            return this._deserializer.DeserializeEmployees(json);
        }

        public async Task<CheckinDetails> WhenLastSeen(int employeeId)
        {
            this.CheckToken();

            var relativePath = $"last_seen.php?zone=KBP&employeeId={employeeId}";
            var response = await this._httpClient.GetAsync(relativePath);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Failed to get last seen info for employee {employeeId}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return this._deserializer.DeserializeCheckinDetails(json);
        }

        public async Task<CheckinStats> TotalOfficeTimeToday(int employeeId)
        {
            this.CheckToken();

            var relativePath = $"events.php?zone=KBP&employeeId={employeeId}&from={TimeProvider.TodayMilliseconds}&till={TimeProvider.NowMilliseconds}";
            var response = await this._httpClient.GetAsync(relativePath);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Failed to get total office time for today for employee {employeeId}");
            }

            var json = await response.Content.ReadAsStringAsync();
            List<CheckinEvent> checkinEvents = (List<CheckinEvent>)this._deserializer.DeserializeCheckinsEvents(json);
            
            return new CheckinStatsCalculator(checkinEvents).Calculate();
        }


        private void CheckToken()
        {
            if (!this.IsTokenSet)
            {
                throw new BotCommandException("Token was not set");
            }
        }
    }
}