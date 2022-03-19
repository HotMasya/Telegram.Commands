namespace Masya.Telegram.Commands.Exceptions
{
    public sealed class InvalidModuleException : Exception
    {
        public Type ModuleType { get; }

        public InvalidModuleException(string message, Type moduleType) : base(message)
        {
            ModuleType = moduleType;
        }

        public override string ToString()
        {
            return $"{Message} Module: {ModuleType.FullName}";
        }
    }
}