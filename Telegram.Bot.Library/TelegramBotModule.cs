using Autofac;
using Telegram.Bot.Library.Commands;

namespace Telegram.Bot.Library
{
    public class TelegramBotModule : Module
    {
        private readonly string _botToken;

        public TelegramBotModule(string botToken)
        {
            _botToken = botToken;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TelegramBot>().WithParameter(new NamedParameter("token", this._botToken)).SingleInstance();
            builder.RegisterType<BotCommandExecutor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BotCommandsFactory>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
