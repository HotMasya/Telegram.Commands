namespace Masya.Telegram.Commands.Exceptions
{
    public sealed class UnknownCommandException : Exception
    {
        public string Command { get; }
        public CommandService CommandService { get; }

        public UnknownCommandException(string message, string command, CommandService commandService)
            : base(message)
        {
            Command = command;
            CommandService = commandService;
        }

        public override string ToString()
        {
            return $"{Message} Command: {Command}";
        }
    }
}