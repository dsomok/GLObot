using System;
using Autofac;
using Serilog;
using Telegram.Bot.GLObot.Notifier.Commands;
using Telegram.Bot.GLObot.Notifier.GLO;
using Telegram.Bot.GLObot.Notifier.GLO.Checkins;
using Telegram.Bot.GLObot.Notifier.GLO.Employees;
using Telegram.Bot.GLObot.Notifier.GLO.Serialization;
using Telegram.Bot.GLObot.Notifier.PredefinedEmployees;
using Telegram.Bot.Library;

namespace Telegram.Bot.GLObot.Notifier
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = BuildDependencies(
                botToken: "382577872:AAGzP16-emtd8TQTJka-KBfvjmctJDGGS7s",
                pollingPeriod: TimeSpan.FromMinutes(5)
            );


            var officeTimeClient = container.Resolve<GLOOfficeTimeClient>();
            var employeesRegistry = container.Resolve<IEmployeesRegistry>();

            officeTimeClient.OnSetToken(() => employeesRegistry.PopulateEmployees().Wait());

            using (var bot = container.Resolve<TelegramBot>())
            {
                bot.Start();
                Console.ReadLine();
            }
        }

        static IContainer BuildDependencies(string botToken, TimeSpan pollingPeriod)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new LoggerConfiguration().WriteTo.Console().CreateLogger())
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterType<EmployeesRegistry>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GLOOfficeTimePollerRegistry>().WithParameter(new NamedParameter("pollingPeriod", pollingPeriod)).SingleInstance();
            builder.RegisterType<GLOOfficeTimeClient>().SingleInstance();
            builder.RegisterType<Deserializer>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PredefinedEmployeesRegistry>().SingleInstance();
            builder.RegisterType<PredefinedEmployeesKeyboard>().SingleInstance();
            builder.RegisterModule<CommandsModule>();
            builder.RegisterModule(new TelegramBotModule(botToken));

            return builder.Build();
        }
    }
}
