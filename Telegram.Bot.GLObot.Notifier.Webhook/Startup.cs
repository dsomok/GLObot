﻿using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.GLObot.Notifier.Webhook.Commands;

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
            services.AddTelegramBot<GloBot>(_configuration.GetSection("GloBot"))
                .AddUpdateHandler<EchoCommand>()
                .AddUpdateHandler<StartCommand>()
                .AddUpdateHandler<SetTokenCommand>()
                .AddUpdateHandler<EmployeeTrackHandler>()
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
            var logger = new LoggerConfiguration().CreateLogger();
            builder.RegisterInstance(logger).AsImplementedInterfaces().SingleInstance();
            builder.RegisterInstance(_configuration).AsImplementedInterfaces().SingleInstance();
        }
    }
}