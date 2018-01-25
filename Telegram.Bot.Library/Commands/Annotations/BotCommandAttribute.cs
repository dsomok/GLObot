using System;

namespace Telegram.Bot.Library.Commands.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BotCommandAttribute : Attribute
    {
        public BotCommandAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}