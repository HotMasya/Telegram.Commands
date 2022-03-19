namespace Masya.Telegram.Commands.Metadata
{
    public class ModuleInfo
    {
        public Type ModuleType { get; }
        public IReadOnlyDictionary<string, CommandInfo> Commands { get; }

        public ModuleInfo(Type moduleType, IReadOnlyDictionary<string, CommandInfo> commands)
        {
            ModuleType = moduleType;
            Commands = commands;
        }
    }
}