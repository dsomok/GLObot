using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.GLObot.Notifier.Webhook.Commands;
using Telegram.Bot.Library;

namespace Telegram.Bot.GLObot.Notifier.Webhook
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var botConfiguration = _configuration.GetSection("GloBot");
            services.AddTelegramBot<GloBot>(new BotOptions<GloBot>()
                {
                    ApiToken = botConfiguration["ApiToken"],
                    BotUserName = botConfiguration["BotUserName"],
                    PathToCertificate = botConfiguration["PathToCertificate"],
                    WebhookUrl = $"https://{_configuration["VIRTUAL_HOST"]}/bots/{{bot}}/webhook/{{token}}"
                })
                .AddUpdateHandler<EchoCommand>()
                .AddUpdateHandler<StartCommand>()
                .AddUpdateHandler<SetTokenCommand>()
                .AddUpdateHandler<EmployeeTrackHandler>()
                .AddUpdateHandler<KeyboardMarkupCommand>()
                .Configure();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger logger)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            logger.Information("Setting webhook for {BotName}...", nameof(GloBot));
            app.ApplicationServices.GetRequiredService<IBotManager<GloBot>>();

            app.UseTelegramBotWebhook<GloBot>();
            logger.Information("Webhook is set for bot {BotName}",nameof(GloBot));

            app.Run(async context => { await context.Response.WriteAsync("Hello World!"); });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var logger = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Code).CreateLogger();
            builder.RegisterInstance(logger).AsImplementedInterfaces().SingleInstance();
            builder.RegisterInstance(_configuration).AsImplementedInterfaces().SingleInstance();
            builder.RegisterModule<BotModule>();
        }
    }
}