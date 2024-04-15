using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Collections.Concurrent;
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
                Console.WriteLine(message.Text);
                var userTag = $"@{message.Chat.Username!}";
                var userId = message.Chat.Id.ToString();
                switch (message.Text)
                {
                    case "/start":
                    {
                        await repo.AddConnectId(userTag, userId);
                        if (_authenticationTriggersStart.TryGetValue(userTag, out _))
                            _authenticationTriggersStart[userTag].Release();
                    } break;
                    case "Подтвердить личность":
                    {
                        if (_authenticationTriggersConfirm.TryGetValue(userTag, out _))
                            _authenticationTriggersConfirm[userTag].Release();
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


    private async Task CheckConfirmUser(string telegramTag)
    {
        await SendButton(await repo.GetUserId(telegramTag));
        if (!_authenticationTriggersConfirm.ContainsKey(telegramTag))
            _authenticationTriggersConfirm[telegramTag] = new SemaphoreSlim(0);
        if (!await _authenticationTriggersConfirm[telegramTag].WaitAsync(TimeSpan.FromSeconds(10)))
        {
            // tempData["error"] = "Превышение времени ожидания";
            Console.WriteLine("Превышение времени ожидания");
            _authenticationTriggersConfirm.TryRemove(telegramTag, out _);
            return;
        }
        await repo.AddAuthenticatedUser(telegramTag);
    }

    private async Task CheckStartUser(string telegramTag)
    {
        if (!_authenticationTriggersStart.ContainsKey(telegramTag))
            _authenticationTriggersStart[telegramTag] = new SemaphoreSlim(0);
        else
        {
            // tempData["error"] = "Наш бот ждет вас";
            return;
        }

        if (!await _authenticationTriggersStart[telegramTag].WaitAsync(TimeSpan.FromSeconds(5)))
        {
            // tempData["error"] = "Превышение времени ожидания";
            Console.WriteLine("Превышение времени ожидания");
            _authenticationTriggersStart.TryRemove(telegramTag, out _);
        }
    }
    
    public async Task AuthenticationUser(string telegramTag)
    {
        if (await repo.CheckUser(telegramTag))
        {
            await CheckConfirmUser(telegramTag);
            return;
        }
        await CheckStartUser(telegramTag);
        await CheckConfirmUser(telegramTag);
    }

    private async Task SendButton(string telegramId)
    {
        var confirmButton = new KeyboardButton("Подтвердить личность");
        var replyMarkup = new ReplyKeyboardMarkup(new[] { confirmButton });
        await botClient.SendTextMessageAsync(int.Parse(telegramId), "1", replyMarkup: replyMarkup);
    }
}