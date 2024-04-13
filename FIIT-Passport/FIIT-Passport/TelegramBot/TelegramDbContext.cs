using Fiit_passport.Databased;
using Fiit_passport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiit_passport.TelegramBot;

public class TelegramDbContext
{
    private readonly ApplicationDbContext _dbContext;

    public TelegramDbContext(ApplicationDbContext db)
    {
        _dbContext = db;
    }

    public async Task AddConnectId(string userTag, string userId)
    {
        if (await CheckUser(userTag))
            return;
        await _dbContext.ConnectIds.AddAsync(new ConnectId(userTag, userId));
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> CheckUser(string userTag)
    {
        var user = await _dbContext.ConnectIds.FindAsync(userTag);
        return user is not null;
    }

    public async Task<string> GetUserId(string userTag)
    {
        var user = await _dbContext.ConnectIds.FindAsync(userTag);
        return user!.UserTelegramId;
    }

    public async Task CreatePassport(string sessionId)
    {
        await _dbContext.Passports.AddAsync(new Passport(sessionId));
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> CheckPassport(string idSession)
    {
        var passport = await _dbContext.Passports.FindAsync(idSession);
        return passport is not null;
    }

    public async Task<Passport?> GetPassport(string idSession) => await _dbContext.Passports.FindAsync(idSession);

    public async Task UpdatePassport(Passport passport)
    {
        if (!await CheckPassport(passport.SessionId!))
            return;
        var ps = await GetPassport(passport.SessionId);
        _dbContext.Passports.Remove(ps);
        await _dbContext.Passports.AddAsync(passport);
        await _dbContext.SaveChangesAsync();
    }
}