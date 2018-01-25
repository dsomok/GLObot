using Autofac;
using Telegram.Bot.Library.Commands;

namespace Telegram.Bot.GLObot.Notifier.Commands
{
    class CommandsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(HelpCommand).Assembly).Where(t => typeof(IBotCommand).IsAssignableFrom(t));
        }
    }
}
