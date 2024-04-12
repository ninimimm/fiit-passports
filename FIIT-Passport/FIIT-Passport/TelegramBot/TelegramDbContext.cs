using Fiit_passport.Databased;
using Fiit_passport.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fiit_passport.TelegramBot;

public class TelegramDbContext
{
    private readonly ApplicationDbContext? _dbContext;

    public TelegramDbContext(ApplicationDbContext db)
    {
        _dbContext = db;
    }

    public async Task AddConnectId(string userTag, string userId)
    {
        if (await CheckUser(userTag))
            return;
        await _dbContext!.ConnectIds.AddAsync(new ConnectId(userTag, userId));
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> CheckUser(string userTag)
    {
        var user = await _dbContext!.ConnectIds.FindAsync(userTag);
        return user is not null;
    }

    public async Task<string> GetUserId(string userTag)
    {
        var user = await _dbContext!.ConnectIds.FindAsync(userTag);
        return user!.UserTelegramId;
    }
}