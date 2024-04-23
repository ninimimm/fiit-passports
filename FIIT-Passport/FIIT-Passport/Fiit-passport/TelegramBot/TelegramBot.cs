using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Fiit_passport.TelegramBot;

public class TelegramBot(TelegramDbContext repo, ITelegramBotClient botClient)
{

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
                        await botClient.SendTextMessageAsync(userId, "Нажмите кнопку проверить личность на сайте", cancellationToken: cancellationToken);
                    } break;
                    default:
                    {
                        if (await repo.CheckPassport(message.Text))
                        {
                            var passport = await repo.GetPassport(message.Text);
                            passport!.AuthenticatedTelegramTag = userTag;
                            await repo.UpdatePassport(passport);
                            await botClient.SendTextMessageAsync(userId, "Аутентификация пройдена", cancellationToken: cancellationToken);
                        }
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

    public async Task SendButton(string telegramId, string sessionId)
    {
        var confirmButton = new KeyboardButton(sessionId);
        var replyMarkup = new ReplyKeyboardMarkup(new[] { confirmButton });
        await botClient.SendTextMessageAsync(int.Parse(telegramId), "Нажмите кнопку подтвердить", replyMarkup: replyMarkup);
    }
}