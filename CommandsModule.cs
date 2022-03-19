using Masya.Telegram.Commands.Abstractions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Masya.Telegram.Commands
{
    public class CommandsModule : ICommandsModule
    {
        public IModuleContext? Context { get; init; }

        public async Task<Message> SendTextMessageAsync(
            string text,
            ParseMode? parseMode = null,
            IEnumerable<MessageEntity>? entities = null,
            bool? disableWebPagePreview = null,
            bool? disableNotification = null,
            int? replyToMessageId = null,
            bool? allowSendingWithoutReply = null,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default
        )
        {
            if(Context == null)
            {
                throw new InvalidOperationException("Context was null.");
            }

            return await Context.Client.SendTextMessageAsync(
                Context.Message.Chat.Id,
                text,
                parseMode,
                entities,
                disableWebPagePreview,
                disableNotification,
                replyToMessageId,
                allowSendingWithoutReply,
                replyMarkup,
                cancellationToken
            );
        }
    }
}
