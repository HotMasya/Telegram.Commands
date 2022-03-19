namespace Masya.Telegram.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class CommandAttribute : Attribute
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public CommandAttribute(string name)
        {
            Name = name;
        }
    }
}