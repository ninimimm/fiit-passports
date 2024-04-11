using Fiit_passport.Databased;
using Fiit_passport.Models;

namespace Fiit_passport.TelegramBot;

public class TelegramDbContext
{
    private static ApplicationDbContext? DbContext { get; set; }

    private protected TelegramDbContext(ApplicationDbContext db)
    {
        DbContext = db;
    }

    public static async Task AddConnectId(string userTag, string userId)
    {
        if (await CheckUser(userTag))
            return;
        await DbContext!.ConnectIds.AddAsync(new ConnectId(userTag, userId));
        await DbContext.SaveChangesAsync();
    }

    public static async Task<bool> CheckUser(string userTag)
    {
        var user = await DbContext!.ConnectIds.FindAsync(userTag);
        return user is not null;
    }

    public static async Task<string> GetUserId(string userTag)
    {
        var user = await DbContext!.ConnectIds.FindAsync(userTag);
        return user!.UserTelegramId;
    }
}