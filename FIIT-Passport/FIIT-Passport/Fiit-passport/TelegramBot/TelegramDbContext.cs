﻿using Fiit_passport.Database;
using Fiit_passport.Models;
using Microsoft.EntityFrameworkCore;

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

    private async Task<int> GetMaxNumber(Status status)
    {
        var sessionNumbers = await db.SessionNumbers.Where(x => x.Status == status).ToListAsync();
        return sessionNumbers.Count != 0 ? sessionNumbers.Max(x => x.Number) : 0;
    }

    public async Task CreateSessionNumber(string sessionId, string name)
    {
        if (await db.SessionNumbers.FindAsync(sessionId) is not null)
            return;
        var sessionNumber = new SessionNumber(sessionId,await GetMaxNumber(Status.SendToReview) + 1, Status.SendToReview, name);
        await db.SessionNumbers.AddAsync(sessionNumber);
        await db.SaveChangesAsync();
    }

    private async Task<SessionNumber?> GetSessionNumber(string? idSession) => await db.SessionNumbers.FindAsync(idSession);
    
    public async Task UpdateSessionNumber(string sessionId, int number, Status status, string name)
    {
        var sessionNumber = new SessionNumber(sessionId, number, status, name);
        var sn = await GetSessionNumber(sessionId);
        var passport = await GetPassport(sessionId);
        passport!.Status = status;
        await UpdatePassport(passport);
        sn!.Update(sessionNumber);
        await db.SaveChangesAsync();
    }
    
    public async Task<List<SessionNumber>> GetAllSessionNumbers() =>
        await db.SessionNumbers.ToListAsync();
    
    public async Task<Passport?> GetPassport(string? idSession) => await db.Passports.FindAsync(idSession);

    public async Task<List<Passport>> GetPassports(string authenticatedTelegramTag) =>
        await db.Passports.Where(x => x.AuthenticatedTelegramTag == authenticatedTelegramTag).ToListAsync();

    public async Task UpdatePassport(Passport passport)
    {
        if (!await CheckPassport(passport.SessionId!))
            return;
        var ps = await GetPassport(passport.SessionId!);
        ps!.Update(passport);
        await db.SaveChangesAsync();
    }

    public async Task<int> CreateComment(string sessionId, string name, int start, int end, string text)
    {
        var newId = 1;
        if (db.Comments.Any())
            newId += await db.Comments.MaxAsync(x => x.Id);
        var comment = new Comment(sessionId, name, start, end, text, newId);
        await db.Comments.AddAsync(comment);
        await db.SaveChangesAsync();
        return newId;
    }

    public async Task UpdateComment(int id, string text)
    {
        var comment = await db.Comments.FindAsync(id);
        if (comment is null) return;
        comment.Update(text);
        await db.SaveChangesAsync();
    }

    public async Task DeleteComment(int id)
    {
        var comment = await GetComment(id);
        db.Comments.Remove(comment!);
        await db.SaveChangesAsync();
    }

    private async Task<Comment?> GetComment(int id) =>
        await db.Comments.FindAsync(id);
    
    public async Task<List<Comment>> GetCommentsBySessionId(string sessionId) =>
        await db.Comments.Where(c => c.SessionId == sessionId).ToListAsync();
}