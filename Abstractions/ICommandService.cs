using System.Reflection;

namespace Masya.Telegram.Commands.Abstractions
{
    public interface ICommandService
    {
        ITelegramBotClient Client { get; }
        IServiceProvider? Services { get; }
        bool ThrowOnUnknownCommand { get; }

        event LogEventHandler? OnLog;

        Task AddModulesAsync(Assembly assembly);
        Task HandleCommandAsync(Message message);
        
    }
}