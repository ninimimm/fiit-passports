using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Telegram.Bot.Types.ReplyMarkups;

namespace Fiit_passport.TelegramBot;

public class TelegramBot(TelegramDbContext repo, ITelegramBotClient botClient)
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _authenticationTriggersStart = new();
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _authenticationTriggersConfirm = new();

    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
            return;
        var message = update.Message;
        try
        {
            if (message!.Text is not null)
            {
                var userTag = $"@{message.Chat.Username!}";
                var userId = message.Chat.Id.ToString();
                switch (message.Text)
                {
                    case "/start":
                    {
                        await repo.AddConnectId(userTag, userId);
                        await SendButton(userId);
                    } break;
                    case "Подтвердить личность":
                    {
                        await repo.AddAuthenticatedUser(userTag);
                    } break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        return Task.CompletedTask;
    }

    public async Task SendButton(string telegramId)
    {
        var confirmButton = new KeyboardButton("Подтвердить личность");
        var replyMarkup = new ReplyKeyboardMarkup(new[] { confirmButton });
        await botClient.SendTextMessageAsync(int.Parse(telegramId), "1", replyMarkup: replyMarkup);
    }
}