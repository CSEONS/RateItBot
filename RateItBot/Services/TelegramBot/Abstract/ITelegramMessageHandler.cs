using Telegram.Bot.Types;
using Telegram.Bot;

namespace RateItBot.Services.TelegramBot.Abstract
{
    public interface ITelegramMessageHandler
    {
        Task HandleAsync(Update update, ITelegramBotClient botClient, IServiceProvider serviceProvider);
    }
}
