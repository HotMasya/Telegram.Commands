namespace Masya.Telegram.Commands.Metadata
{
    public class ParameterInfo
    {
        public string Name { get; }
        public Type Type { get; }

        public ParameterInfo(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}