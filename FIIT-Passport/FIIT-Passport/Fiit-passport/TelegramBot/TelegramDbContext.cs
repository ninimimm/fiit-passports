using Fiit_passport.Database;
using Fiit_passport.Models;

namespace Fiit_passport.TelegramBot;

public class TelegramDbContext(ApplicationDbContext db)
{
    public async Task AddConnectId(string userTag, string userId)
    {
        if (await CheckUser(userTag))
            return;
        await db.ConnectIds.AddAsync(new ConnectId(userTag, userId));
        await db.SaveChangesAsync();
    }

    public async Task<bool> CheckUser(string userTag)
    {
        var user = await db.ConnectIds.FindAsync(userTag);
        return user is not null;
    }

    public async Task<string> GetUserId(string userTag)
    {
        var user = await db.ConnectIds.FindAsync(userTag);
        return user!.UserTelegramId;
    }

    public async Task<Passport> CreatePassport(string? sessionId)
    {
        var passport = new Passport(sessionId);
        await db.Passports.AddAsync(passport);
        await db.SaveChangesAsync();
        return passport;
    }

    public async Task<bool> CheckPassport(string? idSession)
    {
        var passport = await db.Passports.FindAsync(idSession);
        return passport is not null;
    }

    public async Task<Passport?> GetPassport(string? idSession) => await db.Passports.FindAsync(idSession);

    public async Task UpdatePassport(Passport passport)
    {
        if (!await CheckPassport(passport.SessionId!))
            return;
        var ps = await GetPassport(passport.SessionId!);
        ps!.Update(passport);
        // db.Passports.Remove(passport);
        // await db.Passports.AddAsync(passport);
        await db.SaveChangesAsync();
    }
}