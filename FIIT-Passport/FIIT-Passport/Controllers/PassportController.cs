﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fiit_passport.Models;
using Fiit_passport.TelegramBot;
using Microsoft.Net.Http.Headers;

namespace Fiit_passport.Controllers;

public class PassportController(TelegramDbContext repo, TelegramBot.TelegramBot botTools) : Controller
{
    public string CreateIdSession() => Guid.NewGuid().ToString();

    [HttpPost]
    public async Task<IActionResult> AuthenticationUser(string telegramTag)
    {
        await botTools.AuthenticationUser(telegramTag, TempData);
        return RedirectToAction("UpdatePassport");
    }

    private void AddCookie(string key, string value)
    {
        var options = new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddYears(10)
        };
        Response.Cookies.Append(key, value, options);
    }
    public string? GetCookie(string key) => Request.Cookies[key];

    public async Task<IActionResult> SaveAndRedirect(string pageName, Passport? passport)
    {
        if (passport is null)
            return RedirectToAction(pageName);
        await repo.UpdatePassport(passport);
        return RedirectToAction(pageName, await repo.GetPassport(passport.SessionId));
    }
    
    public IActionResult HomePage() => View();
    
    public async Task<IActionResult> AboutProject()
    {
        var idSession = GetCookie("idSession");
        if (idSession is not null && await repo.CheckPassport(idSession))
            return View(await repo.GetPassport(idSession));
        idSession = CreateIdSession();
        AddCookie("idSession", idSession);
        await repo.CreatePassport(idSession);
        return View(await repo.GetPassport(idSession));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutProjectToDetails(Passport passport) =>
        await SaveAndRedirect( "DetailsProject", passport);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutProjectToHome(Passport passport) =>
        await SaveAndRedirect("HomePage", passport);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DetailsProjectToAbout(Passport passport) =>
        await SaveAndRedirect("AboutProject", passport);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DetailsProjectToOthers(Passport passport) =>
        await SaveAndRedirect("OthersProject", passport);
    
    public IActionResult DetailsProject(Passport passport) => View(passport);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OthersProjectToDetails(Passport passport) =>
        await SaveAndRedirect("DetailsProject", passport);
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OthersProjectToContacts(Passport passport) =>
        await SaveAndRedirect("Contacts", passport);
    
    public IActionResult OthersProject(Passport passport) => View(passport);
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactsToOthers(Passport passport) =>
        await SaveAndRedirect("OthersProject", passport);
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactsToCheck(Passport passport) =>
        await SaveAndRedirect("CheckRequest", passport);
    
    public IActionResult Contacts(Passport passport) => View(passport);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckToContacts(Passport passport) =>
        await SaveAndRedirect("Contacts", passport);
    public IActionResult CheckRequest(Passport passport) => View(passport);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassport1 (Passport passport)
    {
        await repo.UpdatePassport(passport);
        return RedirectToAction("");
    }
    
    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}