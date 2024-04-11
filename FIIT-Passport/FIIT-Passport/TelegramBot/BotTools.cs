using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Fiit_passport.TelegramBot;

public static class BotTools
{
    public static readonly ConcurrentDictionary<string, SemaphoreSlim> AuthenticationTriggers;
    public static readonly ConcurrentDictionary<string, string> TelegramUserIdMapping;

    static BotTools()
    {
        AuthenticationTriggers = new ConcurrentDictionary<string, SemaphoreSlim>();
        TelegramUserIdMapping = new ConcurrentDictionary<string, string>();
    }
    
    private static class Authentication
    {
        private static async Task AuthenticationUser(string telegramTag,  ITempDataDictionary tempData)
        {
            if (await TelegramDbContext.CheckUser(telegramTag))
            {
                await SendButton(await TelegramDbContext.GetUserId(telegramTag));
                await AuthenticationTriggers[telegramTag].WaitAsync();
                return;
            }
                
            
            if (!AuthenticationTriggers.ContainsKey(telegramTag))
                AuthenticationTriggers[telegramTag] = new SemaphoreSlim(0);
            else
            {
                tempData["error"] = "Наш бот ждет вас";
                return;
            }

            if (!await AuthenticationTriggers[telegramTag].WaitAsync(TimeSpan.FromHours(1)))
            {
                tempData["error"] = "Превышение времени ожидания";
                AuthenticationTriggers.TryRemove(telegramTag, out _);
                return;
            }
            await SendButton(await TelegramDbContext.GetUserId(telegramTag));

            await AuthenticationTriggers[telegramTag].WaitAsync();
        }

        private static async Task SendButton(string telegramId)
        {
            var confirmButton = new KeyboardButton("Подтвердить личность");
            var replyMarkup = new ReplyKeyboardMarkup(new[] { confirmButton });
            await TelegramBot.BotClient.SendTextMessageAsync(int.Parse(telegramId), null!, replyMarkup: replyMarkup);
        }
    }
}