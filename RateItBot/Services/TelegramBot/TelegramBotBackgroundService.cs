using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RateItBot.TelegramBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

public class TelegramBotBackgroundService : BackgroundService
{
    private readonly ILogger<TelegramBotBackgroundService> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceProvider _serviceProvider;

    public TelegramBotBackgroundService(
        ILogger<TelegramBotBackgroundService> logger,
        ITelegramBotClient botClient,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _botClient = botClient;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _botClient.StartReceiving(OnMessageUpdate, OnError);

        _logger.LogInformation("==Telegram Bot started receiving messages==");

        await Task.CompletedTask;
    }

    private async Task OnMessageUpdate(ITelegramBotClient client, Update update, CancellationToken token)
    {
        if (update.Message != null)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handlerManager = scope.ServiceProvider.GetRequiredService<MessageHandlerManager>();
                await handlerManager.HandleMessageAsync(update, client, _serviceProvider);
            }
        }
    }

    private Task OnError(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        _logger.LogError($"Error: {exception.Message}");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Telegram Bot stopped.");
        await base.StopAsync(stoppingToken);
    }
}
