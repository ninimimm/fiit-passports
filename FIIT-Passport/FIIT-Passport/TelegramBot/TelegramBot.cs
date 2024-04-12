using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;


namespace Fiit_passport.TelegramBot;

public class TelegramBot
{
    private readonly BotTools _botTools;
    
    public TelegramBot(BotTools botTools)
    {
        _botTools = botTools;
    }
    
    private static readonly ConfigurationManager Configuration = new ();
    
    private static readonly string Token = Configuration.GetSection("TelegramSecrets")["Token"]!;
    public static readonly ITelegramBotClient BotClient = new TelegramBotClient(Token);
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
            return;
        var message = update.Message;
        try
        {
            if (message!.Text is not null)
            {
                Console.WriteLine(message.Text);
                var userTag = message.Chat.Username!;
                var userId = message.Chat.Id.ToString();
                switch (message.Text)
                {
                    case "/start":
                    {
                        await _botTools.Repo.AddConnectId($"@{userTag}", userId);
                        if (_botTools.AuthenticationTriggers.TryGetValue(userTag, out _))
                        {
                            _botTools.TelegramUserIdMapping[userTag] = userId;
                            _botTools.AuthenticationTriggers[userTag].Release();
                        }
                    } break;
                    case "Подтвердить личность":
                    {
                        if (_botTools.AuthenticationTriggers.TryGetValue(userTag, out _))
                            _botTools.AuthenticationTriggers[userTag].Release();
                    } break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        return Task.CompletedTask;
    }
}