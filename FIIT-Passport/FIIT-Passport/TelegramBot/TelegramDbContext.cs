using Fiit_passport.Databased;
using Fiit_passport.Models;

namespace Fiit_passport.TelegrammBot;

public class TelegramDbContext
{
    private static ApplicationDbContext? DbContext { get; set; }

    private protected TelegramDbContext(ApplicationDbContext db)
    {
        DbContext = db;
    }

    public static void AddConnectId(string userTag, string userId)
    {
        if (CheckUser(userTag, out _))
            return;
        DbContext!.ConnectIds.Add(new ConnectId(userTag, userId));
    }

    public static bool CheckUser(string userTag, out string userId)
    {
        var user = DbContext!.ConnectIds.Find(userTag);
        if (user is null)
        {
            userId = string.Empty;
            return false;
        }
        userId = user.UserTelegramId;
        return true;
    }
}