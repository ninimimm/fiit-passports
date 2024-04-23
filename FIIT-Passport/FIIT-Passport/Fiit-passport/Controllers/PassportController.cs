using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fiit_passport.Models;
using Fiit_passport.TelegramBot;
using Microsoft.Net.Http.Headers;

namespace Fiit_passport.Controllers;

public class PassportController(TelegramDbContext repo, TelegramBot.TelegramBot botTools) : Controller
{
    public string CreateIdSession() => Guid.NewGuid().ToString();
    
    public async Task<IActionResult> AuthenticationUser(Passport passport)
    {
        var correctPassport = await repo.GetPassport(passport.SessionId);
        if (!await repo.CheckUser(passport.TelegramTag!))
            TempData["error"] = $"Наш бот ждет команды /start от пользователя {passport.TelegramTag}";
        else if (correctPassport!.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            TempData["error"] = $"{passport.TelegramTag} нажмите подтвердить \"Подтвердить\"";
            await botTools.SendButton(await repo.GetUserId(passport.TelegramTag!), passport.SessionId!);
        }
        else
        {
            TempData["success"] = "Теперь всё готово к отправке проекта";
        }
            
        return await SaveAndRedirect("CheckRequest", passport);
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
    
    public void DeleteCookie(string key) => Response.Cookies.Delete(key);

    public async Task<IActionResult> SaveAndRedirect(string pageName, Passport? passport)
    {
        if (passport is null)
            return RedirectToAction(pageName);
        await repo.UpdatePassport(passport);
        return RedirectToAction(pageName, await repo.GetPassport(passport.SessionId));
    }
    
    public IActionResult HomePage() => View(GetCookie("idSession") is not null);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutProject(bool isNewProject)
    {
        var idSession = GetCookie("idSession");
        if (!isNewProject && idSession is not null && await repo.CheckPassport(idSession))
            return View("AboutProject", await repo.GetPassport(idSession));
        idSession = CreateIdSession();
        DeleteCookie("idSession");
        AddCookie("idSession", idSession);
        await repo.CreatePassport(idSession);
        return View("AboutProject", await repo.GetPassport(idSession));
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
    public async Task<IActionResult> DetailsProjectToAbout(Passport passport)
    {
        await repo.UpdatePassport(passport);
        return await AboutProject(false);
    }

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
    public async Task<IActionResult> CheckToHome(Passport passport)
    {
        var correctPassport = await repo.GetPassport(passport.SessionId);
        if (passport.TelegramTag is null)
        {
            TempData["error"] = "Имя пользователя telegram не может быть пустым";
            return await SaveAndRedirect("CheckRequest", passport);
        }
        if (correctPassport.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            TempData["error"] = $"Сначала подтвердите свою личность для пользователя {passport.TelegramTag}";
            return await SaveAndRedirect("CheckRequest", passport);
        }
        DeleteCookie("idSession");
        return await SaveAndRedirect("HomePage", passport);
    }
    
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