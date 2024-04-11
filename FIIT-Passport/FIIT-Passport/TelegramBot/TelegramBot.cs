using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;


namespace Fiit_passport.TelegrammBot;

public static class TelegramBot
{

    private static readonly ConfigurationManager Configuration = new ();
    
    private static readonly string Token = Configuration.GetTelegramSecrets("Token")!;
    public static readonly ITelegramBotClient BotClient = new TelegramBotClient(Token);
    private static readonly CancellationToken CancellationToken = new CancellationTokenSource().Token;

    private static string? GetTelegramSecrets(this ConfigurationManager configuration, string name) =>
        configuration.GetSection("Telegram")[name];
    
    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
            return;
        var message = update.Message;
        try
        {
            if (message!.Text is not null)
            {
                var userTag = message.Chat.Username!;
                var userId = message.Chat.Id.ToString();
                switch (message.Text)
                {
                    case "/start":
                    {
                        TelegramDbContext.AddConnectId(userTag, userId);
                        if (BotTools.AuthenticationTriggers.TryGetValue(userTag, out _))
                        {
                            BotTools.TelegramUserIdMapping[userTag] = userId;
                            BotTools.AuthenticationTriggers[userTag].Release();
                        }
                    } break;
                    case "Подтвердить личность":
                    {
                        if (BotTools.AuthenticationTriggers.TryGetValue(userTag, out _))
                            BotTools.AuthenticationTriggers[userTag].Release();
                    } break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
    }

    public static async void RunBot()
    {
        var me = BotClient.GetMeAsync().Result;
        BotClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync));
        Console.ReadLine();
        try { await Task.Delay(-1, CancellationToken); }
        catch (TaskCanceledException) { }
    }
}