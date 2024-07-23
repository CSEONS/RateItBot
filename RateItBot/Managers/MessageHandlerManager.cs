using Telegram.Bot.Types;
using Telegram.Bot;
using RateItBot.Services.TelegramBot.Abstract;

namespace RateItBot.TelegramBot
{
    public class MessageHandlerManager
    {
        private readonly IEnumerable<ITelegramMessageHandler> _handlers;

        public MessageHandlerManager(IEnumerable<ITelegramMessageHandler> handlers)
        {
            _handlers = handlers;
        }

        public async Task HandleMessageAsync(Update update, ITelegramBotClient botClient, IServiceProvider serviceProvider)
        {
            foreach (var handler in _handlers)
            {
                await handler.HandleAsync(update, botClient, serviceProvider);
            }
        }

    }
}
