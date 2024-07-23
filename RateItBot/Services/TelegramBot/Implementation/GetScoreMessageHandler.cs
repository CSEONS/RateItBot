using RateItBot.Managers;
using RateItBot.Services.TelegramBot.Abstract;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RateItBot.Services.TelegramBot.Implementation
{
    public class GetScoreMessageHandler : ITelegramMessageHandler
    {
        private readonly RatingManager _ratingManager;
        private readonly string _commandText = "/score";


        private string[] _topMessages = [
            "{0}. {1}[Мистер виш]🐟 {2}",
            "{0}. {1}[Мемолог]😂 {2}",
            "{0}. {1}[Так себе шутник]🥸 {2}",
            "{0}. {1}[Не сдавшийся]🧑🏻‍🦽 {2}"
        ];

        public GetScoreMessageHandler(RatingManager ratingManager)
        {
            _ratingManager = ratingManager;
        }

        public async Task HandleAsync(Update update, ITelegramBotClient botClient, IServiceProvider serviceProvider)
        {
            if (update?.Message?.Text != _commandText)
                return;

            var topUsers = _ratingManager
                .GetTop(3)
                .OrderBy(u => u.Rating.Sum(r => r.Score))
                .Reverse();

            var top = string.Empty;

            for (int i = 0; i < topUsers.Count(); i++)
            {
                top += string.Format(
                    _topMessages[i], 
                    i + 1, 
                    topUsers.ElementAt(i).UserName, 
                    topUsers.ElementAt(i).Rating.Sum(r => r.Score));

                top += "\n";
            }

            await botClient.SendTextMessageAsync(update.Message.Chat.Id, top);
        }
    }
}
