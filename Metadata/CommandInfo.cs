using System.Reflection;

namespace Masya.Telegram.Commands.Metadata
{
    public class CommandInfo
    {
        public string Name { get; }
        public string DisplayName { get; }
        public string? Description { get; }
        public MethodInfo CommandHandler { get; }

        public CommandInfo(string name, string? description, MethodInfo commandHandler)
        {
            Name = name.ToLower();
            DisplayName = name;
            Description = description;
            CommandHandler = commandHandler;
        }
    }
}