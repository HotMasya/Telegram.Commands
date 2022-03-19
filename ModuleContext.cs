using Masya.Telegram.Commands.Abstractions;

namespace Masya.Telegram.Commands
{
    public class ModuleContext : IModuleContext
    {
        public Message Message { get; }

        public User? User { get; }

        public CommandService Commands { get; }

        public ITelegramBotClient Client { get; }

        public ModuleContext(Message message, CommandService commands)
        {
            Message = message;
            User = message.From;
            Commands = commands;
            Client = commands.Client;
        }
    }
}
