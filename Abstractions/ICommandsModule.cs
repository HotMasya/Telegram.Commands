using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Masya.Telegram.Commands.Abstractions
{
    public interface ICommandsModule
    {
        IModuleContext? Context { get; }

        Task<Message> SendTextMessageAsync(
            string text,
            ParseMode? parseMode = null, 
            IEnumerable<MessageEntity>? entities = null,
            bool? disableWebPagePreview = null, 
            bool? disableNotification = null, 
            int? replyToMessageId = null, 
            bool? allowSendingWithoutReply = null, 
            IReplyMarkup? replyMarkup = null, 
            CancellationToken cancellationToken = default
        );
    }
}