namespace Masya.Telegram.Commands.Abstractions
{
    public interface IModuleContext
    {
        Message Message { get; }
        User? User { get; }
        CommandService Commands { get; }
        ITelegramBotClient Client { get; }
    }
}