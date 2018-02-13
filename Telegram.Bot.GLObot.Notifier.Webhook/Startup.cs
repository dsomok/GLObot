using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.GLObot.Notifier.Webhook.Commands;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO.Serialization;
using Telegram.Bot.GLObot.Notifier.Webhook.PredefinedEmployees;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTelegramBot<GloBot>(_configuration.GetSection("GloBot"))
                .AddUpdateHandler<EchoCommand>()
                .AddUpdateHandler<StartCommand>()
                .AddUpdateHandler<SetTokenCommand>()
                .AddUpdateHandler<EmployeeTrackHandler>()
                .Configure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new TelegramBotModule("474477823:AAFMWjLjncdWzgTlgHaQs2qY1daz88sAQQg"));
            var logger = new LoggerConfiguration().CreateLogger();
            builder.RegisterInstance(logger).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GloOfficeTimeClient>().SingleInstance();
            builder.RegisterType<PredefinedEmployeesRegistry>().SingleInstance();
            builder.RegisterType<PredefinedEmployeesKeyboard>().SingleInstance();
            builder.RegisterType<Deserializer>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterInstance(_configuration).AsImplementedInterfaces().SingleInstance();
        }
    }
}