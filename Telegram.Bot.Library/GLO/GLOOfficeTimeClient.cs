using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Telegram.Bot.Library.Exceptions;
using Telegram.Bot.Library.Extensions;
using Telegram.Bot.Library.GLO.Checkins;
using Telegram.Bot.Library.GLO.Employees;
using Telegram.Bot.Library.GLO.Serialization;
using Telegram.Bot.Library.GLO.Serialization.Types;
using Telegram.Bot.Library.Services;

namespace Telegram.Bot.Library.GLO
{
    internal class GloOfficeTimeClient : IGloOfficeTimeClient
    {
        private readonly IDeserializer _deserializer;
        private readonly HttpClient _httpClient;

        private string _token;
        private readonly IList<Action> _onSetTokenActions = new List<Action>();
        private readonly ILogger _logger;

        public GloOfficeTimeClient(IDeserializer deserializer, BotConfiguration configuration, ILogger logger)
        {
            _deserializer = deserializer;
            _logger = logger;
            this._httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.OfficeTimeConfiguration.Url)
            };

            var preconfiguredToken = configuration.OfficeTimeConfiguration.AuthenticationToken;
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
            _logger.Information("Querying last seen info on employee {employeeId}", employeeId);
            var relativePath = $"last_seen.php?zone=KBP&employeeId={employeeId}";
            var response = await this._httpClient.GetAsync(relativePath);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Failed to get last seen info for employee {employeeId}");
            }

            var json = await response.Content.ReadAsStringAsync();
            _logger.Information("Received response on employee {employeeId}", employeeId);
            return this._deserializer.DeserializeCheckinDetails(json);
        }

        public async Task<CheckinStats> TotalOfficeTimeToday(int employeeId)
        {
            this.CheckToken();

            _logger.Information("Querying total time on employee {employeeId}", employeeId);
            var relativePath = $"events.php?zone=KBP&employeeId={employeeId}&from={TimeProvider.TodayMilliseconds}&till={TimeProvider.NowMilliseconds}";

            HttpResponseMessage response;
            using (var taskCTS = new CancellationTokenSource())
            {
                response = await this._httpClient.GetAsync(relativePath, taskCTS.Token).TimeoutAfter(TimeSpan.FromSeconds(6), taskCTS);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Failed to get total office time for today for employee {employeeId}");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            List<CheckinEvent> checkinEvents = (List<CheckinEvent>)this._deserializer.DeserializeCheckinsEvents(json);

            _logger.Information("Received response on employee {employeeId}", employeeId);
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