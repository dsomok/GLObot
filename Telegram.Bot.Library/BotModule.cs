using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Telegram.Bot.Library.GLO;
using Telegram.Bot.Library.GLO.Serialization;
using Telegram.Bot.Library.PredefinedEmployees;
using Telegram.Bot.Library.Repositories;

namespace Telegram.Bot.Library
{
    public class BotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BotConfiguration>().AsSelf().SingleInstance();
            builder.RegisterType<InmemoryEmployeesRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GloOfficeTimeClient>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EmployeesRegistry>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<Deserializer>().AsImplementedInterfaces().SingleInstance();
            base.Load(builder);
        }
    }
}
