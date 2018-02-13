using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO.Checkins;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO.Employees;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO.Serialization;
using Telegram.Bot.Library.Exceptions;

namespace Telegram.Bot.GLObot.Notifier.Webhook.GLO
{
    class GloOfficeTimeClient
    {
        private readonly IDeserializer _deserializer;
        private readonly HttpClient _httpClient;

        private string _token;
        private readonly IList<Action> _onSetTokenActions = new List<Action>();

        public GloOfficeTimeClient(IDeserializer deserializer, IConfiguration configuration)
        {
            _deserializer = deserializer;
            this._httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://portal-ua.globallogic.com/officetime/json/")
            };

            var preconfiguredToken = configuration["token"];
            if (preconfiguredToken != null)
                SetToken(preconfiguredToken);
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



        private void CheckToken()
        {
            if (!this.IsTokenSet)
            {
                throw new BotCommandException("Token was not set");
            }
        }
    }
}