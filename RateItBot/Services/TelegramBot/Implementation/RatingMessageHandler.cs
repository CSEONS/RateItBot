using RateItBot.Domain.Entities;
using RateItBot.Domain.Repositories.Abstract;
using RateItBot.Managers;
using RateItBot.Services.TelegramBot.Abstract;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

public class RatingMessageHandler : ITelegramMessageHandler
{
    private readonly Regex _ratingRegex = new Regex(@"\d{1,2}/\d{1,2}");
    private readonly IUserRepository _userRepository;
    private readonly UserManager _userManager;
    private readonly RatingManager _ratingManager;

    public RatingMessageHandler(IUserRepository userRepository, UserManager userManager, RatingManager ratingManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _ratingManager = ratingManager;
    }

    public async Task HandleAsync(Update update, ITelegramBotClient botClient, IServiceProvider serviceProvider)
    {
        if (update?.Message?.ReplyToMessage != null)
        {
            if (string.IsNullOrEmpty(update.Message.Text))
                return;

            var text = update.Message.Text;

            var match = _ratingRegex.Match(text);
            if (match.Success is false)
                return;

            var ratingParts = match.Value.Split('/');
            if (ratingParts.Length != 2)
                return;

            if (float.TryParse(ratingParts[0], out float leftNumber) && float.TryParse(ratingParts[1], out float rightNumber))
            {
                if (rightNumber <= 0)
                    return;

                if (leftNumber > rightNumber)
                    return;

                if (leftNumber < 0)
                    return;

                if (update.Message.ReplyToMessage.From?.Id == null)
                    return;

                RateItBot.Domain.Entities.User user = null;
                if (_userManager.ExistByTelegramId(update.Message.ReplyToMessage.From.Id) == false)
                {
                    user = _userManager.Create
                    (
                        telegrmaId: update.Message.ReplyToMessage.From.Id,
                        username: update.Message.ReplyToMessage.From.Username
                    );
                }
                else
                {
                    user = _userRepository.GetByTelegramId(update.Message.ReplyToMessage.From.Id);
                }

                if (update.Message.ReplyToMessage.From.Id == update.Message?.From?.Id)
                    return;

                _ratingManager.Add(user, leftNumber, rightNumber);
            }
        }
    }


}
