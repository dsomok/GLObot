using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Telegram.Bot.Library.Commands.Annotations;

namespace Telegram.Bot.Library.Commands
{
    internal class BotCommandsFactory : IBotCommandsFactory
    {
        private readonly IDictionary<string, Func<IBotCommand>> _commandsRegistry;

        public BotCommandsFactory(IComponentContext resolver)
        {
            this._commandsRegistry = (from registration in resolver.ComponentRegistry.Registrations
                let type = registration.Activator.LimitType
                let name = registration.Activator.LimitType.GetCustomAttribute<BotCommandAttribute>()?.Name
                where !string.IsNullOrEmpty(name) &&
                      typeof(IBotCommand).IsAssignableFrom(type)
                select new KeyValuePair<string, Func<IBotCommand>>(
                    key: name,
                    value: () => resolver.Resolve(type) as IBotCommand
                )).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public IReadOnlyList<string> KnownCommands => this._commandsRegistry.Keys.ToList<string>();

        public IBotCommand Create(string commandName)
        {
            if (!this._commandsRegistry.TryGetValue(commandName, out Func<IBotCommand> commandFactory))
            {
                return null;
            }

            return commandFactory();
        }
    }
}