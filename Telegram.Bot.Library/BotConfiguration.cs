using System.Configuration;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Telegram.Bot.Library
{
    public class BotConfiguration
    {
        private readonly IConfiguration _configuration;

        public BotConfiguration(IConfiguration config)
        {
            OfficeTimeConfiguration = new OfficeTimeConfiguration(config["token"]);
            _configuration = config;
        }

        public OfficeTimeConfiguration OfficeTimeConfiguration { get; }

        public void EnsureConfigurationIsValid()
        {
            var requiredKeys = new[] {"token", "Globot:ApiToken", "Globot:BotUserName", "Globot:WebhookUrl"};
            foreach (var requiredKey in requiredKeys)
                if (_configuration[requiredKey] == null)
                    throw new ConfigurationErrorsException($"Configuration key {requiredKey} is required. Required parameters: {string.Join(", ", requiredKeys)}");
        }
    }

    public class OfficeTimeConfiguration
    {
        internal OfficeTimeConfiguration(string authenticationToken)
        {
            AuthenticationToken = authenticationToken;
        }

        public string AuthenticationToken { get; }
        public string Url => "https://portal-ua.globallogic.com/officetime/json/";
    }
}